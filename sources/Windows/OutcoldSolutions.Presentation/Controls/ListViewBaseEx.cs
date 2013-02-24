// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Controls
{
    using System;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Extensions methods for LisViewBase control..
    /// </summary>
    public static class ListViewBaseEx
    {
        /// <summary>
        /// Scroll to horizontal offset <paramref name="horizontalOffset"/>.
        /// </summary>
        /// <param name="listViewBase">
        /// The list view base control.
        /// </param>
        /// <param name="horizontalOffset">
        /// The horizontal offset.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static void ScrollToHorizontalOffset(this ListViewBase listViewBase, double horizontalOffset)
        {
            if (listViewBase == null)
            {
                throw new ArgumentNullException("listViewBase");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(listViewBase);
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
        /// <param name="listViewBase">
        /// The list view base.
        /// </param>
        /// <param name="verticalOffset">
        /// The vertical offset.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static void ScrollToVerticalOffset(this ListViewBase listViewBase, double verticalOffset)
        {
            if (listViewBase == null)
            {
                throw new ArgumentNullException("listViewBase");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(listViewBase);
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
        /// <param name="listViewBase">
        /// The list view base control.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static void ScrollToHorizontalZero(this ListViewBase listViewBase)
        {
            ScrollToHorizontalOffset(listViewBase, 0.0d);
        }

        /// <summary>
        /// Scroll to vertical zero.
        /// </summary>
        /// <param name="listViewBase">
        /// The list view base control.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static void ScrollToVerticalZero(this ListViewBase listViewBase)
        {
            ScrollToVerticalOffset(listViewBase, 0.0d);
        }

        /// <summary>
        /// Get scroll viewer horizontal offset.
        /// </summary>
        /// <param name="listViewBase">
        /// The list view base control.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static double GetScrollViewerHorizontalOffset(this ListViewBase listViewBase)
        {
            if (listViewBase == null)
            {
                throw new ArgumentNullException("listViewBase");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(listViewBase);
            if (scrollViewer != null)
            {
                return scrollViewer.HorizontalOffset;
            }

            return 0.0d;
        }

        /// <summary>
        /// Get scroll viewer vertical offset.
        /// </summary>
        /// <param name="listViewBase">
        /// The list view base control.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="listViewBase"/> is null.
        /// </exception>
        public static double GetScrollViewerVerticalOffset(this ListViewBase listViewBase)
        {
            if (listViewBase == null)
            {
                throw new ArgumentNullException("listViewBase");
            }

            var scrollViewer = VisualTreeHelperEx.GetVisualChild<ScrollViewer>(listViewBase);
            if (scrollViewer != null)
            {
                return scrollViewer.VerticalOffset;
            }

            return 0.0d;
        }
    }
}