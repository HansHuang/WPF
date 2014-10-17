using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {


    public MainWindow()
    {
      InitializeComponent();
      Box.Command = new RelayCommand(s => Console.WriteLine("Hello"));
      //Loaded += MainWindow_Loaded;
    }

    //void MainWindow_Loaded(object sender, RoutedEventArgs e)
    //{
    //  var hotKeyHost = new HotKeyHost((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
    //  hotKeyHost.AddHotKey(new CustomHotKey("ShowPopup", Key.W, ModifierKeys.Control | ModifierKeys.Alt, true));
    //  hotKeyHost.AddHotKey(new CustomHotKey("ClosePopup", Key.F2, ModifierKeys.Control, true));
    //}
  }


  [Serializable]
  public class CustomHotKey : HotKey
  {
    #region Name (INotifyPropertyChanged Property)

    private string _name;

    public string Name {
      get { return _name; }
      set {
        if (_name != null && _name.Equals(value)) return;
        _name = value;
        RaisePropertyChanged("Name");
      }
    }

    #endregion

    public CustomHotKey(string name, Key key, ModifierKeys modifiers, bool enabled)
      : base(key, modifiers, enabled)
    {
      Name = name;
    }


    protected override void OnHotKeyPress()
    {
      MessageBox.Show(string.Format("'{0}' has been pressed ({1})", Name, this));

      base.OnHotKeyPress();
    }


    protected CustomHotKey(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
      : base(info, context)
    {
      Name = info.GetString("Name");
    }

    public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      base.GetObjectData(info, context);

      info.AddValue("Name", Name);
    }
  }


}
