using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ItemsControlGrid
{
  public class GridItemsControl : ItemsControl
  {
    public GridItemsControl()
    {
      DefaultStyleKey = typeof(GridItemsControl);
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
    }
  }
}
