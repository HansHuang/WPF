using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicGrid
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      DataSource = new ObservableCollection<string> {"AAAAAAAAAA", "BBBBBBBBBB"};
      InitializeComponent();
    }

    public static readonly DependencyProperty DataSourceProperty =
      DependencyProperty.Register("DataSource", typeof (ObservableCollection<string>), typeof (MainWindow), new PropertyMetadata(default(ObservableCollection<string>)));

    public ObservableCollection<string> DataSource {
      get { return (ObservableCollection<string>) GetValue(DataSourceProperty); }
      set { SetValue(DataSourceProperty, value); }
    }
  }
}
