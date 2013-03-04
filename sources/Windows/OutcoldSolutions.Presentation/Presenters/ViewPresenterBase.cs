// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Views;

    /// <summary>
    /// The ViewPresenterBase interface.
    /// </summary>
    internal interface IViewPresenterBase
    {
        void Initialize(IView view);
    }

    /// <summary>
    /// The view presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of view.
    /// </typeparam>
    public class ViewPresenterBase<TView> : BindingModelBase, IViewPresenterBase
        where TView : IView
    {
        internal IDependencyResolverContainer Container { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        public TView View { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        protected IDispatcher Dispatcher { get; private set; }

        /// <inheritdoc />
        void IViewPresenterBase.Initialize(IView view)
        {
            this.View = (TView)view;
            this.OnInitialized();
        }

        [Inject]
        internal void InjectMethod(
            IDependencyResolverContainer container,
            ILogManager logManager, 
            IDispatcher dispatcher)
        {
            this.Logger = logManager.CreateLogger(this.GetType().Name);
            this.Dispatcher = dispatcher;
            this.Container = container;
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }
    }
}
