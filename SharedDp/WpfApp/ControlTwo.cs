using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    internal class ControlTwo : ContentControl
    {
        #region DependencyProperty ItemTag

        public static readonly DependencyProperty ItemTagProperty =
            EntityItem.ItemTagProperty.AddOwner(typeof (ControlTwo));

        public string ItemTag
        {
            get { return (string) GetValue(ItemTagProperty); }
            set { SetValue(ItemTagProperty, value); }
        }

        #endregion
        
        #region DependencyProperty Title
        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (ControlTwo), new PropertyMetadata(default(string), OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (ControlTwo)d;
            host.Content = host.ItemTag + ": " + e.NewValue;
        }

        #endregion
    }
}
