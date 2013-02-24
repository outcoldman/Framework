// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System.Threading.Tasks;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;

    /// <summary>
    /// The application base.
    /// </summary>
    public abstract class ApplicationBase : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase"/> class.
        /// </summary>
        protected ApplicationBase()
        {
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IDependencyResolverContainer Container { get; private set; }

        /// <summary>
        /// The on search activated.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            this.InitializeInternal();

            base.OnSearchActivated(args);
        }

        /// <summary>
        /// The on launched.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.InitializeInternal();

            base.OnLaunched(args);
        }

        /// <summary>
        /// The initialize application.
        /// </summary>
        protected abstract void InitializeApplication();

        /// <summary>
        /// The on suspending async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected abstract Task OnSuspendingAsync();

        private void InitializeInternal()
        {
            if (Container == null)
            {
                Container = new DependencyResolverContainer();
                this.InitializeApplication();
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            var deferral = suspendingEventArgs.SuspendingOperation.GetDeferral();

            var suspendingTask = this.OnSuspendingAsync();
            if (suspendingTask != null)
            {
                suspendingTask.ContinueWith((t) => deferral.Complete());
            }
            else
            {
                deferral.Complete();
            }
        }
    }
}