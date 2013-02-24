// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    /// <summary>
    /// The page view base.
    /// </summary>
    public class PageViewBase : ViewBase, IPageView
    {
        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService { get; private set; }

        /// <inheritdoc />
        public virtual void OnNavigatedTo(NavigatedToEventArgs eventArgs)
        {
            ((IPagePresenterBase)this.DataContext).OnNavigatedTo(eventArgs);
        }

        /// <inheritdoc />
        public virtual void OnNavigatingFrom(NavigatingFromEventArgs eventArgs)
        {
            ((IPagePresenterBase)this.DataContext).OnNavigatingFrom(eventArgs);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.NavigationService = this.Container.Resolve<INavigationService>();
        }
    }
}
