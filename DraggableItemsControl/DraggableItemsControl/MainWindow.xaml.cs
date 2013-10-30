using System;
using System.Collections.Generic;
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

namespace DragAndDropItemsControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ViewModel();
        }

        #region ViewModel DependencyProperty
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ViewModel), typeof(MainWindow), new PropertyMetadata(default(ViewModel)));

        public ViewModel ViewModel
        {
            get { return (ViewModel)GetValue(ViewModelProperty); }
            private set { SetValue(ViewModelProperty, value); }
        } 
        #endregion
    }
}
