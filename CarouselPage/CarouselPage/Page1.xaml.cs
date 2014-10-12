using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CarouselPageDemo
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : UserControl
    {
        [Export(typeof(ICarouselPage))]
        public ICarouselPage CarouselPage { get; private set; }

        public Page1()
        {
            InitializeComponent();
            CarouselPage = new CarouselPage("Page 1", this);
        }
    }
}
