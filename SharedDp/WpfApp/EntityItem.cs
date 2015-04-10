using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    internal class EntityItem
    {
        #region DependencyProperty ItemTag
        public static void SetItemTag(UIElement element, string value)
        {
            element.SetValue(ItemTagProperty, value);
        }

        public static string GetItemTag(UIElement element)
        {
            return (string)element.GetValue(ItemTagProperty);
        }

        public static readonly DependencyProperty ItemTagProperty = DependencyProperty.RegisterAttached(
            "ItemTag", typeof(string), typeof(EntityItem),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits)); 
        #endregion


    }
}
