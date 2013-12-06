using System.Collections.Specialized;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;

namespace ItemsControlGrid
{
  public class GridUtils
  {
    #region ItemsPerRow attached property

    /// <summary>
    /// Identifies the ItemsPerRow attached property. 
    /// </summary>
    public static readonly DependencyProperty ItemsPerRowProperty =
        DependencyProperty.RegisterAttached("ItemsPerRow", typeof(int), typeof(GridUtils),
            new PropertyMetadata(0, new PropertyChangedCallback(OnItemsPerRowPropertyChanged)));

    /// <summary>
    /// Gets the value of the ItemsPerRow property
    /// </summary>
    public static int GetItemsPerRow(DependencyObject d)
    {
      return (int)d.GetValue(ItemsPerRowProperty);
    }

    /// <summary>
    /// Sets the value of the ItemsPerRow property
    /// </summary>
    public static void SetItemsPerRow(DependencyObject d, int value)
    {
      d.SetValue(ItemsPerRowProperty, value);
    }

    /// <summary>
    /// Handles property changed event for the ItemsPerRow property, constructing
    /// the required ItemsPerRow elements on the grid which this property is attached to.
    /// </summary>
    private static void OnItemsPerRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
    {
      var grid = (Grid) d;
      var itemsPerRow = (int)e.NewValue;

      // construct the required row definitions
      grid.LayoutUpdated += (s, e2) =>
        {
          // iterate over any new content presenters (i.e. instances of our DataTemplte)
          // that have been added to the grid
          var presenters = grid.Children.OfType<ContentPresenter>().ToList();
          foreach (var presenter in presenters)
          {
            // the child of each DataTemplate should be our 'phantom' panel
            var phantom = VisualTreeHelper.GetChild(presenter, 0) as PhantomPanel;
            if (phantom == null) continue;
            // remove each of the children of the phantom and add to the grid
            foreach (var child in phantom.Children.OfType<FrameworkElement>().ToList())
            {
              phantom.Children.Remove(child);
              grid.Children.Add(child);
              // ensure the child maintains its original datacontext
              child.DataContext = phantom.DataContext;
            }

            // remove the presenter (and phantom)
            grid.Children.Remove(presenter);
          }

          var childCount = grid.Children.Count;
          var rowDifference = (childCount / itemsPerRow) - grid.RowDefinitions.Count;

          // if new items have been added, create the required grid rows
          // and assign the row index to each child
          if (rowDifference != 0)
          {
            grid.RowDefinitions.Clear();
            for (int row = 0; row < (childCount / itemsPerRow); row++)
            {
              grid.RowDefinitions.Add(new RowDefinition());
            }

            // set the row property for each chid
            for (int i = 0; i < childCount; i++)
            {
              var child = grid.Children[i] as FrameworkElement;
              Grid.SetRow(child, i / itemsPerRow);
            }
          }
        };
    }

    #endregion


    /// <summary>
    /// Identified the ItemsSource attached property
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable), typeof(GridUtils),
            new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

    /// <summary>
    /// Gets the value of the ItemsSource property
    /// </summary>
    public static IEnumerable GetItemsSource(DependencyObject d)
    {
      return (IEnumerable)d.GetValue(ItemsSourceProperty);
    }

    /// <summary>
    /// Sets the value of the ItemsSource property
    /// </summary>
    public static void SetItemsSource(DependencyObject d, IEnumerable value)
    {
      d.SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Handles property changed event for the ItemsSource property.
    /// </summary>
    private static void OnItemsSourcePropertyChanged(DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      var control = d as ItemsControl;

      // set the ItemsSource of the ItemsControl that this property is attached to
      control.ItemsSource = e.NewValue as IEnumerable;

      var notifyCollection = e.NewValue as INotifyCollectionChanged;
      if (notifyCollection != null)
      {
        // if a collection changed event occurs, reset the ItemsControl's
        // ItemsSource, rebuilding the UI 
        notifyCollection.CollectionChanged += (s, e2) =>
          {
            control.ItemsSource = null;
            control.ItemsSource = e.NewValue as IEnumerable;
          };
      }
    }
    
  }
}
