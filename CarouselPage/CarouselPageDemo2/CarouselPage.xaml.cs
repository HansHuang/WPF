using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarouselPageDemo2
{
    /// <summary>
    /// Interaction logic for CarouselPage.xaml
    /// </summary>
    public partial class CarouselPage : UserControl, INotifyPropertyChanged
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

        #region Pages

        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
            "Pages", typeof (ObservableCollection<UIElement>), typeof (CarouselPage),
            new PropertyMetadata(null, PagesChanged));

        private static void PagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CarouselPage;
            if (ctrl == null) return;
            var old = e.OldValue as ObservableCollection<UIElement>;
            var neW = e.NewValue as ObservableCollection<UIElement>;
            if (old != null) old.CollectionChanged -= ctrl.PagesCollectionChanged;
            if (neW != null) neW.CollectionChanged += ctrl.PagesCollectionChanged;
        }

        public ObservableCollection<UIElement> Pages
        {
            get { return (ObservableCollection<UIElement>) GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

        #endregion

        #region ActivedPage (INotifyPropertyChanged Property)

        private UIElement _activedPage;

        public UIElement ActivedPage
        {
            get { return _activedPage; }
            set
            {
                if (_activedPage != null && _activedPage.Equals(value)) return;
                _activedPage = value;
                RaisePropertyChanged("ActivedPage");
            }
        }

        #endregion

        #region AnimaterPage (INotifyPropertyChanged Property)

        private UIElement _AnimaterPage;

        public UIElement AnimaterPage
        {
            get { return _AnimaterPage; }
            set
            {
                if (_AnimaterPage != null && _AnimaterPage.Equals(value)) return;
                _AnimaterPage = value;
                RaisePropertyChanged("AnimaterPage");
            }
        }

        #endregion

        #region RelayCommand NavegateCmd

        private RelayCommand _navegateCmd;

        public ICommand NavegateCmd
        {
            get { return _navegateCmd ?? (_navegateCmd = new RelayCommand(NavegateExecute)); }
        }

        private int _lastIndex;

        private void NavegateExecute(object index)
        {
            int idx;
            int.TryParse(index + "", out idx);
            if (idx < 0 || idx == _lastIndex || Pages == null || idx > Pages.Count) return;
            if (Viewer == null) return;
            ActivedPage = Pages[idx];
            AnimaterPage = Pages[_lastIndex];
            if (_lastIndex < idx)
                Viewer.BeginStoryboard((Storyboard) Resources["SlideLeftToRight"]);
            else
                Viewer.BeginStoryboard((Storyboard) Resources["SlideRightToLeft"]);

            _lastIndex = idx;
        }

        #endregion

        public CarouselPage()
        {
            Pages = new ObservableCollection<UIElement>();
            Pages.CollectionChanged += PagesCollectionChanged;
            InitializeComponent();
        }

        private void PagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ActivedPage == null)
                ActivedPage = Pages.FirstOrDefault();
        }

    }
}
