using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public class ControlOne : ContentControl
    {
        #region DependencyProperty ItemTag
        public string ItemTag
        {
            get { return (string)GetValue(ItemTagProperty); }
            set { SetValue(ItemTagProperty, value); }
        }

        public static readonly DependencyProperty ItemTagProperty =
            EntityItem.ItemTagProperty.AddOwner(typeof(ControlOne));
        #endregion

        #region DependencyProperty Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        } 

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(ControlOne), new PropertyMetadata(default(object), OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (ControlOne) d;
            host.Content = host.ItemTag + ": " + e.NewValue;
        }
        #endregion
    }
}
