// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using OutcoldSolutions.Diagnostics;

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

    /// <summary>
    /// The page presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of view.
    /// </typeparam>
    /// <typeparam name="TBindingModel">
    /// The type of binding model.
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Class with the same name.")]
    public abstract class PagePresenterBase<TView, TBindingModel> : PagePresenterBase<TView>
        where TView : IDataPageView 
        where TBindingModel : BindingModelBase
    {
        private readonly IDependencyResolverContainer container;

        private TBindingModel bindingModel;

        private bool isDataLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagePresenterBase{TView,TBindingModel}"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        protected PagePresenterBase(IDependencyResolverContainer container)
            : base(container)
        {
            this.container = container;
            this.Toolbar = container.Resolve<IApplicationToolbar>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether is data loading.
        /// </summary>
        public bool IsDataLoading
        {
            get
            {
                return this.isDataLoading;
            }

            protected set
            {
                this.isDataLoading = value;
                this.RaiseCurrentPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the binding model.
        /// </summary>
        public TBindingModel BindingModel
        {
            get
            {
                return this.bindingModel;
            }

            private set
            {
                this.bindingModel = value;
                this.RaiseCurrentPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the toolbar.
        /// </summary>
        protected IApplicationToolbar Toolbar { get; private set; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigatedToEventArgs parameter)
        {
            base.OnNavigatedTo(parameter);

            this.IsDataLoading = true;
            this.View.OnDataLoading(parameter);
            this.BindingModel.FreezeNotifications();

            this.Logger.LogTask(Task.Factory.StartNew(
                async () =>
                    {
                        await this.LoadDataAsync(parameter);

                        await this.Dispatcher.RunAsync(
                            () =>
                                {
                                    this.Toolbar.SetViewCommands(this.GetViewCommands());
                                    this.View.OnUnfreeze(parameter);
                                    this.BindingModel.UnfreezeNotifications();
                                    this.IsDataLoading = false;
                                });
                        
                        await Task.Delay(10);
                        await this.Dispatcher.RunAsync(() => this.View.OnDataLoaded(parameter));
                    }));
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.BindingModel = this.container.Resolve<TBindingModel>();
        }

        /// <summary>
        /// The load data async.
        /// </summary>
        /// <param name="navigatedToEventArgs">
        /// The navigated to event args.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected abstract Task LoadDataAsync(NavigatedToEventArgs navigatedToEventArgs);

        /// <summary>
        /// The get view commands.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{CommandMetadata}"/>.
        /// </returns>
        protected virtual IEnumerable<CommandMetadata> GetViewCommands()
        {
            yield break;
        }
    }
}