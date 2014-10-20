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
        private HotKeyHost _host;

        public MainWindow()
        {
            InitializeComponent();
            Box.Command = new RelayCommand(s => Console.WriteLine("Hello"));
            Box.ClearHotkey+=BoxClearHotkey;
        }

        private void BoxClearHotkey()
        {
            if (_host != null)
                _host.RemoveHotKey(Box.Hotkey);
        }


        private void BtnApplyOnClick(object sender, RoutedEventArgs e)
        {
            _host = _host ?? new HotKeyHost((HwndSource)HwndSource.FromVisual(Application.Current.MainWindow));
            _host.AddHotKey(Box.Hotkey);
        }
    }

}
