using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CarouselPageDemo
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : UserControl
    {
        [Export(typeof(ICarouselPage))]
        public ICarouselPage CarouselPage { get; private set; }

        public Page2()
        {
            InitializeComponent();
            CarouselPage = new CarouselPage("Page 2", this);
        }
    }
}
