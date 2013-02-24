// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using OutcoldSolutions.Diagnostics;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The page base.
    /// </summary>
    public class PageBase : Page, IView
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the container.
        /// </summary>
        protected IDependencyResolverContainer Container { get; private set; }

        /// <summary>
        /// Gets the presenter.
        /// </summary>
        protected PresenterBase Presenter { get; private set; }

        /// <summary>
        /// The get presenter.
        /// </summary>
        /// <typeparam name="TPresenter">
        /// The type of presenter.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TPresenter"/>.
        /// </returns>
        protected TPresenter GetPresenter<TPresenter>() where TPresenter : PresenterBase
        {
            return (TPresenter)this.Presenter;
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        /// <param name="presenterBase">
        /// The presenter base.
        /// </param>
        [Inject]
        protected void Initialize(
            IDependencyResolverContainer container,
            ILogManager logManager,
            PresenterBase presenterBase)
        {
            this.Container = container;
            this.Presenter = presenterBase;
            this.Logger = this.Container.Resolve<ILogManager>().CreateLogger(this.GetType().Name);
            this.DataContext = presenterBase;

            var viewPresenterBase = presenterBase as IViewPresenterBase;
            if (viewPresenterBase != null)
            {
                viewPresenterBase.Initialize(this);
            }

            this.OnInitialized();
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }
    }
}