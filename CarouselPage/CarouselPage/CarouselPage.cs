using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarouselPageDemo
{
    public class CarouselPage : ICarouselPage
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

        #region Title (INotifyPropertyChanged Property)

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != null && _title.Equals(value)) return;
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        #endregion

        #region Page (INotifyPropertyChanged Property)

        private Control _page;

        public Control Page
        {
            get { return _page; }
            set
            {
                if (_page != null && _page.Equals(value)) return;
                _page = value;
                RaisePropertyChanged("Page");
            }
        }

        #endregion

        public CarouselPage() { }
        public CarouselPage(string title, Control page)
        {
            Title = title;
            Page = page;
        }
    }
}
