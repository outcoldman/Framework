// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    using OutcoldSolutions.Views;

    /// <summary>
    /// The menu item metadata.
    /// </summary>
    public class MenuItemMetadata
    {
        /// <summary>
        /// Gets the page type.
        /// </summary>
        public Type PageType { get; private set; }

        /// <summary>
        /// Gets the page resolver type.
        /// </summary>
        public Type PageResolverType { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public object Content { get; private set; }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// The from view type.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="parameter">
        /// The navigation parameter.
        /// </param>
        /// <typeparam name="TPageView">
        /// The view type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MenuItemMetadata"/>.
        /// </returns>
        public static MenuItemMetadata FromViewType<TPageView>(object title, object parameter = null) where TPageView : IPageView
        {
            return new MenuItemMetadata() { PageType = typeof(TPageView), Content = title, Parameter = parameter };
        }

        /// <summary>
        /// The from view type.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="parameter">
        /// The navigation parameter.
        /// </param>
        /// <typeparam name="TPageViewResolver">
        /// The page view type resolver.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MenuItemMetadata"/>.
        /// </returns>
        public static MenuItemMetadata FromPageViewTypeResolver<TPageViewResolver>(object title, object parameter = null) where TPageViewResolver : IPageViewResolver
        {
            return new MenuItemMetadata() { PageResolverType = typeof(TPageViewResolver), Content = title, Parameter = parameter };
        }
    }
}