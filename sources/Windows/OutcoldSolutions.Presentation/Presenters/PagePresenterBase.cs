// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using OutcoldSolutions.Views;

    /// <summary>
    /// The page presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of view.
    /// </typeparam>
    public class PagePresenterBase<TView> : ViewPresenterBase<TView>, IPagePresenterBase
        where TView : IPageView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePresenterBase{TView}"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public PagePresenterBase(
            IDependencyResolverContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        public virtual void OnNavigatedTo(NavigatedToEventArgs parameter)
        {
        }

        /// <inheritdoc />
        public virtual void OnNavigatingFrom(NavigatingFromEventArgs eventArgs)
        {
        }
    }
}