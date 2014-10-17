using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;
using System.Windows.Interop;

namespace Demo
{
  /// <summary>
  /// Represents an hotKey
  /// </summary>
  [Serializable]
  public class HotKey : INotifyPropertyChanged, ISerializable, IEquatable<HotKey>
  {
    #region INotifyPropertyChanged RaisePropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

    #region Key (INotifyPropertyChanged Property)

    private Key _key;

    /// <summary>
    /// The Key. Must not be null when registering to an HotKeyHost.
    /// </summary>
    public Key Key
    {
      get { return _key; }
      set
      {
        if (_key.Equals(value)) return;
        _key = value;
        RaisePropertyChanged("Key");
      }
    }

    #endregion

    #region Modifiers (INotifyPropertyChanged Property)

    private ModifierKeys _modifiers;

    /// <summary>
    /// The modifier. Multiple modifiers can be combined with or.
    /// </summary>
    public ModifierKeys Modifiers
    {
      get { return _modifiers; }
      set
      {
        if (_modifiers.Equals(value)) return;
        _modifiers = value;
        RaisePropertyChanged("Modifiers");
      }
    }

    #endregion

    #region Enabled (INotifyPropertyChanged Property)

    private bool _enabled;

    public bool Enabled
    {
      get { return _enabled; }
      set
      {
        if (_enabled.Equals(value)) return;
        _enabled = value;
        RaisePropertyChanged("Enabled");
      }
    }

    #endregion

    #region Construction methods

    /// <summary>
    /// Creates an HotKey object. This instance has to be registered in an HotKeyHost.
    /// </summary>
    public HotKey()
    {
    }
    
    /// <summary>
    /// Creates an HotKey object. This instance has to be registered in an HotKeyHost.
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="modifiers">The modifier. Multiple modifiers can be combined with or.</param>
    /// <param name="enabled">Specifies whether the HotKey will be enabled when registered to an HotKeyHost</param>
    public HotKey(Key key, ModifierKeys modifiers, bool enabled = true)
    {
      Key = key;
      Modifiers = modifiers;
      Enabled = enabled;
    }

    protected HotKey(SerializationInfo info, StreamingContext context)
    {
      Key = (Key)info.GetValue("Key", typeof(Key));
      Modifiers = (ModifierKeys)info.GetValue("Modifiers", typeof(ModifierKeys));
      Enabled = info.GetBoolean("Enabled");
    }

    #endregion

    #region Override methods of Object

    public override bool Equals(object obj)
    {
      var hotKey = obj as HotKey;
      return hotKey != null && Equals(hotKey);
    }

    public bool Equals(HotKey other)
    {
      return (Key == other.Key && Modifiers == other.Modifiers);
    }

    public override int GetHashCode()
    {
      return (int)Modifiers + 10 * (int)Key;
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      if (Modifiers != ModifierKeys.None) sb.Append(Modifiers);
      if (Key != Key.None)
      {
        if (sb.Length > 0) sb.Append(" + ");
        sb.Append(Key);
      }
      return sb.ToString();
    }

    #endregion

    #region Event: HotKeyPressed

    /// <summary>
    /// Will be raised if the hotkey is pressed (works only if registed in HotKeyHost)
    /// </summary>
    public event EventHandler<HotKeyEventArgs> HotKeyPressed;

    protected virtual void OnHotKeyPress()
    {
      if (HotKeyPressed != null)
        HotKeyPressed(this, new HotKeyEventArgs(this));
    }

    internal void RaiseOnHotKeyPressed()
    {
      OnHotKeyPress();
    }

    #endregion

    #region GetObjectData

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("Key", Key, typeof(Key));
      info.AddValue("Modifiers", Modifiers, typeof(ModifierKeys));
      info.AddValue("Enabled", Enabled);
    }

    #endregion

    
  }

  /// <summary>
  /// The HotKeyHost needed for working with hotKeys.
  /// </summary>
  public sealed class HotKeyHost : IDisposable
  {
    /// <summary>
    /// Creates a new HotKeyHost
    /// </summary>
    /// <param name="hwndSource">The handle of the window. Must not be null.</param>
    public HotKeyHost(HwndSource hwndSource)
    {
      if (hwndSource == null)
        throw new ArgumentNullException("hwndSource");

      _hook = WndProc;
      _hwndSource = hwndSource;
      hwndSource.AddHook(_hook);
    }

    #region HotKey Interop

    private const int WmHotKey = 786;

    [DllImport("user32", CharSet = CharSet.Ansi,
      SetLastError = true, ExactSpelling = true)]
    private static extern int RegisterHotKey(IntPtr hwnd, int id, int modifiers, int key);

    [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern int UnregisterHotKey(IntPtr hwnd, int id);

    #endregion

    #region Interop-Encapsulation

    private readonly HwndSourceHook _hook;
    private readonly HwndSource _hwndSource;

    private void RegisterHotKey(int id, HotKey hotKey)
    {
      if ((int)_hwndSource.Handle == 0) throw new InvalidOperationException("Handle is invalid");
      RegisterHotKey(_hwndSource.Handle, id, (int)hotKey.Modifiers, KeyInterop.VirtualKeyFromKey(hotKey.Key));
      var error = Marshal.GetLastWin32Error();
      if (error == 0) return;
      Exception e = new Win32Exception(error);

      if (error == 1409)
        throw new HotKeyAlreadyRegisteredException(e.Message, hotKey, e);
      throw e;
    }

    private void UnregisterHotKey(int id)
    {
      if ((int)_hwndSource.Handle != 0)
      {
        UnregisterHotKey(_hwndSource.Handle, id);
        int error = Marshal.GetLastWin32Error();
        if (error != 0)
          throw new Win32Exception(error);
      }
    }

    #endregion

    /// <summary>
    /// Will be raised if any registered hotKey is pressed
    /// </summary>
    public event EventHandler<HotKeyEventArgs> HotKeyPressed;

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      if (msg == WmHotKey)
      {
        if (_hotKeys.ContainsKey((int)wParam))
        {
          var h = _hotKeys[(int)wParam];
          h.RaiseOnHotKeyPressed();
          if (HotKeyPressed != null)
            HotKeyPressed(this, new HotKeyEventArgs(h));
        }
      }

      return new IntPtr(0);
    }


    private void HotKeyPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      var kvPair = _hotKeys.FirstOrDefault(h => Equals(h.Value, sender));
      if (kvPair.Value != null)
      {
        if (e.PropertyName == "Enabled")
        {
          if (kvPair.Value.Enabled)
            RegisterHotKey(kvPair.Key, kvPair.Value);
          else
            UnregisterHotKey(kvPair.Key);
        }
        else if (e.PropertyName == "Key" || e.PropertyName == "Modifiers")
        {
          if (kvPair.Value.Enabled)
          {
            UnregisterHotKey(kvPair.Key);
            RegisterHotKey(kvPair.Key, kvPair.Value);
          }
        }
      }
    }


    private readonly Dictionary<int, HotKey> _hotKeys = new Dictionary<int, HotKey>();


    public class SerialCounter
    {
      public SerialCounter(int start)
      {
        Current = start;
      }

      public int Current { get; private set; }

      public int Next()
      {
        return ++Current;
      }
    }

    /// <summary>
    /// All registered hotKeys
    /// </summary>
    public IEnumerable<HotKey> HotKeys
    {
      get { return _hotKeys.Values; }
    }


    private static readonly SerialCounter IDGen = new SerialCounter(1);
    //Annotation: Can be replaced with "Random"-class

    /// <summary>
    /// Adds an hotKey.
    /// </summary>
    /// <param name="hotKey">The hotKey which will be added. Must not be null and can be registed only once.</param>
    public void AddHotKey(HotKey hotKey)
    {
      if (hotKey == null || hotKey.Key == 0)
        throw new ArgumentNullException("hotKey");
      if (_hotKeys.ContainsValue(hotKey))
        throw new HotKeyAlreadyRegisteredException("HotKey already registered!", hotKey);

      var id = IDGen.Next();
      if (hotKey.Enabled)
        RegisterHotKey(id, hotKey);
      hotKey.PropertyChanged += HotKeyPropertyChanged;
      _hotKeys[id] = hotKey;
    }

    /// <summary>
    /// Removes an hotKey
    /// </summary>
    /// <param name="hotKey">The hotKey to be removed</param>
    /// <returns>True if success, otherwise false</returns>
    public bool RemoveHotKey(HotKey hotKey)
    {
      var kvPair = _hotKeys.FirstOrDefault(h => Equals(h.Value, hotKey));
      if (kvPair.Value != null)
      {
        kvPair.Value.PropertyChanged -= HotKeyPropertyChanged;
        if (kvPair.Value.Enabled)
          UnregisterHotKey(kvPair.Key);
        return _hotKeys.Remove(kvPair.Key);
      }
      return false;
    }


    #region Destructor

    private bool _disposed;

    private void Dispose(bool disposing)
    {
      if (_disposed)
        return;

      if (disposing)
      {
        _hwndSource.RemoveHook(_hook);
      }

      for (int i = _hotKeys.Count - 1; i >= 0; i--)
      {
        RemoveHotKey(_hotKeys.Values.ElementAt(i));
      }


      _disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~HotKeyHost()
    {
      Dispose(false);
    }

    #endregion
  }


  public class HotKeyEventArgs : EventArgs
  {
    public HotKey HotKey { get; private set; }

    public HotKeyEventArgs(HotKey hotKey)
    {
      HotKey = hotKey;
    }
  }

  [Serializable]
  public class HotKeyAlreadyRegisteredException : Exception
  {
    public HotKey HotKey { get; private set; }

    public HotKeyAlreadyRegisteredException(string message, HotKey hotKey)
      : base(message)
    {
      HotKey = hotKey;
    }

    public HotKeyAlreadyRegisteredException(string message, HotKey hotKey, Exception inner)
      : base(message, inner)
    {
      HotKey = hotKey;
    }

    protected HotKeyAlreadyRegisteredException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}