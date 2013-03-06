// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Controls
{
    using System.Collections.Generic;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// The visual tree helper extension methods..
    /// </summary>
    public static class VisualTreeHelperEx
    {
        /// <summary>
        /// Get visual child.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <typeparam name="T">
        /// The type of loocking control.
        /// </typeparam>
        /// <returns>
        /// The control.
        /// </returns>
        public static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }

        /// <summary>
        /// Get visual child.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <typeparam name="T">
        /// The type of loocking control.
        /// </typeparam>
        /// <returns>
        /// The control.
        /// </returns>
        public static IEnumerable<T> GetVisualChilds<T>(DependencyObject parent) where T : DependencyObject
        {
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = VisualTreeHelper.GetChild(parent, i);
                T child = v as T;
                if (child == null)
                {
                    foreach (var visualChild in GetVisualChilds<T>(v))
                    {
                        yield return visualChild;
                    }
                }

                if (child != null)
                {
                    yield return child;
                }
            }
        }
    }
}