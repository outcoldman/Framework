// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Views;

    /// <summary>
    /// The DataPagePresenterBase interface.
    /// </summary>
    public interface IDataPagePresenterBase : IPagePresenterBase
    {
        /// <summary>
        /// Gets a value indicating whether is data loading.
        /// </summary>
        bool IsDataLoading { get; }
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
    public abstract class DataPagePresenterBase<TView, TBindingModel> : PagePresenterBase<TView>, IDataPagePresenterBase
        where TView : IDataPageView 
        where TBindingModel : BindingModelBase
    {
        private TBindingModel bindingModel;

        private bool isDataLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPagePresenterBase{TView,TBindingModel}"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        protected DataPagePresenterBase(IDependencyResolverContainer container)
        {
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
                this.SetValue(ref this.isDataLoading, value);
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
                this.SetValue(ref this.bindingModel, value);
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

            this.BindingModel = this.Container.Resolve<TBindingModel>();
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
        /// The <see cref="IEnumerable{IconCommandMetadata}"/>.
        /// </returns>
        protected virtual IEnumerable<CommandMetadata> GetViewCommands()
        {
            yield break;
        }
    }
}