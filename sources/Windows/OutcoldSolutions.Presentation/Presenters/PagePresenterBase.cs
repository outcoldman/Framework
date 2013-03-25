// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Views;

    /// <summary>
    /// The page presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of the view.
    /// </typeparam>
    public abstract class PagePresenterBase<TView> : ViewPresenterBase<TView>, IPagePresenterBase
        where TView : IPageView 
    {
        private bool isDataLoading;

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

        /// <inheritdoc />
        public virtual void OnNavigatedTo(NavigatedToEventArgs parameter)
        {
            this.IsDataLoading = true;
            this.View.OnDataLoading(parameter);
            this.Freeze();

            this.Logger.LogTask(Task.Factory.StartNew(
                async () =>
                {
                    await this.LoadDataAsync(parameter);

                    await this.Dispatcher.RunAsync(
                        () =>
                        {
                            this.MainFrame.SetViewCommands(this.GetViewCommands());
                            this.View.OnUnfreeze(parameter);
                            this.Unfreeze();
                            this.IsDataLoading = false;
                        });

                    await Task.Yield();
                    await this.Dispatcher.RunAsync(() => this.View.OnDataLoaded(parameter));
                }));
        }

        /// <inheritdoc />
        public virtual void OnNavigatingFrom(NavigatingFromEventArgs eventArgs)
        {
        }

        internal virtual void Freeze()
        {
        }

        internal virtual void Unfreeze()
        {
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

    /// <summary>
    /// The page presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The type of view.
    /// </typeparam>
    /// <typeparam name="TBindingModel">
    /// The type of binding model.
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Same logic.")]
    public abstract class PagePresenterBase<TView, TBindingModel> : PagePresenterBase<TView>, IPagePresenterBase
        where TView : IPageView 
        where TBindingModel : BindingModelBase
    {
        private TBindingModel bindingModel;

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

        internal override void Freeze()
        {
            base.Freeze();
            this.BindingModel.FreezeNotifications();
        }

        internal override void Unfreeze()
        {
            base.Unfreeze();
            this.BindingModel.UnfreezeNotifications();
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.BindingModel = this.Container.Resolve<TBindingModel>();
        }
    }
}