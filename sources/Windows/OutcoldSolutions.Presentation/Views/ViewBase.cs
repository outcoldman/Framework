// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The view base.
    /// </summary>
    public class ViewBase : UserControl, IView
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
        protected BindingModelBase Presenter { get; private set; }

        /// <summary>
        /// The get presenter.
        /// </summary>
        /// <typeparam name="TPresenter">
        /// The type of presenter.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TPresenter"/>.
        /// </returns>
        public TPresenter GetPresenter<TPresenter>() 
        {
            return (TPresenter)(object)this.Presenter;
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
            BindingModelBase presenterBase)
        {
            this.Container = container;
            this.Presenter = presenterBase;
            this.Logger = logManager.CreateLogger(this.GetType().Name);
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