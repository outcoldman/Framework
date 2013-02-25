// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;

    /// <summary>
    /// The presenter base.
    /// </summary>
    public class PresenterBase : BindingModelBase
    {
        private readonly IDependencyResolverContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBase"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public PresenterBase(IDependencyResolverContainer container)
        {
            this.container = container;
            this.Logger = this.container.Resolve<ILogManager>().CreateLogger(this.GetType().Name);
            this.Dispatcher = container.Resolve<IDispatcher>();
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        protected IDispatcher Dispatcher { get; private set; }
    }
}
