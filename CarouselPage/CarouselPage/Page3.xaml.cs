using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CarouselPageDemo
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : UserControl
    {
        [Export(typeof(ICarouselPage))]
        public ICarouselPage CarouselPage { get; private set; }

        public Page3()
        {
            InitializeComponent();

            CarouselPage = new CarouselPage("Page 3", this);
        }
    }
}
