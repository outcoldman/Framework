// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Controls
{
    using System;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Extensions methods for ItemsControl control.
    /// </summary>
    public static class ItemsControlExtensions
    {
        /// <summary>
        /// Scroll to horizontal offset <paramref name="horizontalOffset"/>.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base control.
        /// </param>
        /// <param name="horizontalOffset">
        /// The horizontal offset.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static void ScrollToHorizontalOffset(this ItemsControl itemsControl, double horizontalOffset)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(itemsControl);
            if (scrollViewer != null)
            {
                if (scrollViewer.HorizontalScrollMode != ScrollMode.Disabled)
                {
                    scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                }
            }
        }

        /// <summary>
        /// The scroll to vertical offset <paramref name="verticalOffset"/>.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base.
        /// </param>
        /// <param name="verticalOffset">
        /// The vertical offset.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static void ScrollToVerticalOffset(this ItemsControl itemsControl, double verticalOffset)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(itemsControl);
            if (scrollViewer != null)
            {
                if (scrollViewer.VerticalScrollMode != ScrollMode.Disabled)
                {
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }
            }
        }

        /// <summary>
        /// Scroll to horizontal zero.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base control.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static void ScrollToHorizontalZero(this ItemsControl itemsControl)
        {
            ScrollToHorizontalOffset(itemsControl, 0.0d);
        }

        /// <summary>
        /// Scroll to vertical zero.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base control.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static void ScrollToVerticalZero(this ItemsControl itemsControl)
        {
            ScrollToVerticalOffset(itemsControl, 0.0d);
        }

        /// <summary>
        /// Get scroll viewer horizontal offset.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base control.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static double GetScrollViewerHorizontalOffset(this ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(itemsControl);
            if (scrollViewer != null)
            {
                return scrollViewer.HorizontalOffset;
            }

            return 0.0d;
        }

        /// <summary>
        /// Get scroll viewer vertical offset.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base control.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        public static double GetScrollViewerVerticalOffset(this ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(itemsControl);
            if (scrollViewer != null)
            {
                return scrollViewer.VerticalOffset;
            }

            return 0.0d;
        }
    }
}