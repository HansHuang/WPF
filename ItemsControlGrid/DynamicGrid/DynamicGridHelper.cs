using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DynamicGrid
{
  public class DynamicGridHelper
  {

    #region DependencyProperty DataSource
    public static readonly DependencyProperty DataSourceProperty =
      DependencyProperty.RegisterAttached("DataSource", typeof(IEnumerable), typeof(DynamicGridHelper), new PropertyMetadata(null, OnDataSourceChanged));

    public static void SetDataSource(DependencyObject d, int value)
    {
      d.SetValue(DataSourceProperty, value);
    }

    public static IEnumerable GetDataSource(DependencyObject d)
    {
      return (IEnumerable)d.GetValue(DataSourceProperty);
    }


    private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var grid = d as Grid;
      if (grid == null) return;
      var source = e.NewValue;
      if (source == null) return;
      if (!(source is IEnumerable) || !(source is INotifyCollectionChanged)) return;

      grid.RowDefinitions.Clear();
      grid.Children.Clear();
      foreach (var item in (IEnumerable)source) {
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star),MinHeight=30 });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Pixel) });

        var g = new Grid { Background = new SolidColorBrush(Colors.Blue), MinHeight = 30 };
        g.Children.Add(new TextBlock {Text = item.ToString(), Foreground = new SolidColorBrush(Colors.Red)});
        //var row = grid.RowDefinitions.Count - 2;
        Grid.SetRow(g, grid.RowDefinitions.Count - 2);
        grid.Children.Add(g);

        var s = new GridSplitter();
        Grid.SetRow(s, grid.RowDefinitions.Count - 1);
        grid.Children.Add(s);
      }
      grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);
      grid.Children.RemoveAt(grid.Children.Count - 1);

    }
    #endregion

  }
}
