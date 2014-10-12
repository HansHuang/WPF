using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.ComponentModel.Composition;

namespace CarouselPageDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
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

        private int _lastIndex = 0;

        #region CarouselPages (INotifyPropertyChanged Property)
        [ImportMany(typeof(ICarouselPage))]
        private ObservableCollection<ICarouselPage> _carouselPages;

        public ObservableCollection<ICarouselPage> CarouselPages
        {
            get { return _carouselPages; }
            set
            {
                if (_carouselPages != null && _carouselPages.Equals(value)) return;
                _carouselPages = value;
                RaisePropertyChanged("CarouselPages");
            }
        }
        #endregion

        #region ActivePage (INotifyPropertyChanged Property)

        private ICarouselPage _activePage;

        public ICarouselPage ActivePage
        {
            get { return _activePage; }
            set
            {
                if (_activePage != null && _activePage.Equals(value)) return;
                _activePage = value;
                RaisePropertyChanged("ActivePage");
            }
        }

        #endregion

        #region AnimaterPage (INotifyPropertyChanged Property)

        private ICarouselPage _animaterPage;

        public ICarouselPage AnimaterPage
        {
            get { return _animaterPage; }
            set
            {
                if (_animaterPage != null && _animaterPage.Equals(value)) return;
                _animaterPage = value;
                RaisePropertyChanged("AnimaterPage");
            }
        }

        #endregion

        public MainWindow()
        {
            PageComposition();
            //Console.WriteLine(_carouselPages.Count);
            InitializeComponent();
        }

        private void PageComposition()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        private void PointerOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Viewer == null) return;
            AnimaterPage = CarouselPages[_lastIndex];
            if (_lastIndex < Pointer.SelectedIndex)
                Viewer.BeginStoryboard((Storyboard) Resources["SlideLeftToRight"]);
            else
                Viewer.BeginStoryboard((Storyboard) Resources["SlideRightToLeft"]);

            _lastIndex = Pointer.SelectedIndex;
        }
    }
}
