// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    using OutcoldSolutions.Views;

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
        /// The navigate to.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="keepInHistory">
        /// The keep in history.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object NavigateTo(Type type, object parameter = null, bool keepInHistory = true);

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
        IPageView NavigateToView<TViewResolver>(object parameter, bool keepInHistory = true) where TViewResolver : IPageViewResolver;

        /// <summary>
        /// The navigate to view.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="keepInHistory">
        /// The keep in history.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object NavigateToView(Type type, object parameter, bool keepInHistory = true);

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