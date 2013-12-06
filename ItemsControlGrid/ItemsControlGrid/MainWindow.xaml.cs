using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ItemsControlGrid
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private readonly ObservableCollection<ModelObject> _data = new ObservableCollection<ModelObject>(new[] {
      new ModelObject {Item = "Sausages", Quantity = 4},
      new ModelObject {Item = "Eggs", Quantity = 12},
      new ModelObject {Item = "Milk", Quantity = 1},
      new ModelObject {Item = "Bread", Quantity = 2},
    });

    public MainWindow()
    {
      InitializeComponent();

      DataContext = _data;
    }

    private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
    {
      _data.Add(new ModelObject { Item = "Cheese", Quantity = 4 });
    }

    private void ButtonDeleteItem_Click(object sender, RoutedEventArgs e)
    {
      if (_data.Count > 0) _data.RemoveAt(0);
    }
  }

  public class ModelObject : INotifyPropertyChanged
  {
    private string _item;

    private int _quantity;

    public string Item
    {
      get { return _item; }
      set { _item = value; OnPropertyChanged("Item"); }
    }

    public int Quantity
    {
      get { return _quantity; }
      set { _quantity = value; OnPropertyChanged("Quantity"); }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string property)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }
  }
}
