using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropItemsControl
{
    public class DraggableItemsControl : ItemsControl
    {
        #region Format
        //A string that specifies what format to check for.
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(DraggableItemsControl), new PropertyMetadata("MyFromat"));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        } 
        #endregion

        #region IsCopyWhenDrag
        public static readonly DependencyProperty IsCopyWhenDragProperty =
            DependencyProperty.Register("IsCopyWhenDrag", typeof(bool), typeof(DraggableItemsControl), new PropertyMetadata(true));

        public bool IsCopyWhenDrag
        {
            get { return (bool)GetValue(IsCopyWhenDragProperty); }
            set { SetValue(IsCopyWhenDragProperty, value); }
        } 
        #endregion

        private Point _startPoint;

        public DraggableItemsControl() 
        {
            AllowDrop = true;
            //Drag Event
            PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            PreviewMouseMove += OnPreviewMouseMove;

            //Drop Event
            Drop += OnDrop;
            DragEnter += OnDragEnter;
        }

        void OnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(Format) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(Format)) 
            {
                var data = e.Data.GetData(Format);
                var itemsSource = ((DraggableItemsControl)sender).ItemsSource as IList;
                itemsSource.Add(data);
            }
        }

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(null);
        }

        void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            //Only monitor mouse pressed event
            if (e.LeftButton != MouseButtonState.Pressed) return;
            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff = _startPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ItemsControl
                var itemControl = sender as DraggableItemsControl;
                var itemControlItem = FindAnchestor<ContentPresenter>((DependencyObject)e.OriginalSource);

                // Find the data behind the ItemsControl
                var data = itemControl.ItemContainerGenerator.ItemFromContainer(itemControlItem);

                if(!IsCopyWhenDrag)
                    ((IList)itemControl.ItemsSource).Remove(data);

                // Initialize the drag & drop operation
                var dragData = new DataObject(Format, data);
                DragDrop.DoDragDrop(itemControlItem, dragData, DragDropEffects.Move);
            }
        }

        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T) return (T)current;
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
