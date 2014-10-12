using System.ComponentModel;
using System.Windows.Controls;

namespace CarouselPageDemo
{
    public interface ICarouselPage:INotifyPropertyChanged
    {
        string Title { get; }
        Control Page { get; }
    }
}
