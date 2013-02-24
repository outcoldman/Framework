// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    /// <summary>
    /// The ViewPresenterBase interface.
    /// </summary>
    public interface IViewPresenterBase
    {
        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        void Initialize(IView view);
    }

    /// <summary>
    /// The view presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of view.
    /// </typeparam>
    public class ViewPresenterBase<TView> : PresenterBase, IViewPresenterBase
        where TView : IView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewPresenterBase{TView}"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public ViewPresenterBase(IDependencyResolverContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        public TView View { get; private set; }

        /// <inheritdoc />
        void IViewPresenterBase.Initialize(IView view)
        {
            this.View = (TView)view;
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
