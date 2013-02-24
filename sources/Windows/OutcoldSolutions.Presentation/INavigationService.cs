// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The NavigationService interface.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// The navigated to.
        /// </summary>
        event EventHandler<NavigatedToEventArgs> NavigatedTo;

        /// <summary>
        /// The register region provider.
        /// </summary>
        /// <param name="regionProvider">
        /// The region provider.
        /// </param>
        void RegisterRegionProvider(IViewRegionProvider regionProvider);

        /// <summary>
        /// The navigate to.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="keepInHistory">
        /// The keep in history.
        /// </param>
        /// <typeparam name="TView">
        /// Type of the view.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TView"/>.
        /// </returns>
        TView NavigateTo<TView>(object parameter = null, bool keepInHistory = true) where TView : IPageView;

        /// <summary>
        /// The navigate to view.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="keepInHistory">
        /// The keep in history.
        /// </param>
        /// <typeparam name="TViewResolver">
        /// Type resolver.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IPageView"/>.
        /// </returns>
        IPageView NavigateToView<TViewResolver>(object parameter, bool keepInHistory = true) where TViewResolver : IViewResolver;

        /// <summary>
        /// Go back.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Can go back.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CanGoBack();

        /// <summary>
        /// Has history.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool HasHistory();

        /// <summary>
        /// The clear history.
        /// </summary>
        void ClearHistory();
    }
}