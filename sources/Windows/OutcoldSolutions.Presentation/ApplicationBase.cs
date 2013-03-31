// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System.Threading.Tasks;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;
    using OutcoldSolutions.Shell;
    using OutcoldSolutions.Views;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Core;
    using Windows.UI.Xaml;

    /// <summary>
    /// The application base.
    /// </summary>
    public abstract partial class ApplicationBase : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase"/> class.
        /// </summary>
        protected ApplicationBase()
        {
            this.Suspending += this.OnSuspending;
        }
        
        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; private set; }

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
        /// The activated event.
        /// </summary>
        /// <param name="isFirstTimeActivate">
        /// The is first time activate.
        /// </param>
        protected abstract void OnActivated(bool isFirstTimeActivate);

        /// <summary>
        /// The on suspending async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual Task OnSuspendingAsync()
        {
            return null;
        }

        private void InitializeInternal()
        {
            bool isFirstTimeActivate = false;

            MainFrame mainFrame = Window.Current.Content as MainFrame;
            if (mainFrame == null)
            {
                isFirstTimeActivate = true;

                if (ApplicationBase.Container == null)
                {
                    ApplicationBase.Container = new DependencyResolverContainer();

                    using (var registration = ApplicationBase.Container.Registration())
                    {
                        registration.Register<IEventAggregator>().AsSingleton<EventAggregator>();
                        registration.Register<ILogManager>().AsSingleton<LogManager>();
                        registration.Register<INavigationService>().AsSingleton<NavigationService>();
                        registration.Register<IDispatcher>().AsSingleton(new DispatcherContainer(CoreWindow.GetForCurrentThread().Dispatcher));

                        registration.Register<IMainFrame>()
                                    .And<IMainFrameRegionProvider>()
                                    .InjectionRule<BindingModelBase, MainFramePresenter>()
                                    .AsSingleton<MainFrame>();
                        registration.Register<MainFramePresenter>().AsSingleton();

                        registration.Register<IApplicationSettingFrame>()
                                    .InjectionRule<BindingModelBase, ApplicationSettingFramePresenter>()
                                    .As<ApplicationSettingFrame>();
                        registration.Register<ApplicationSettingFramePresenter>();

                        registration.Register<IApplicationSettingViewsService>()
                                    .AsSingleton<ApplicationSettingViewsService>();
                    }

                    this.Logger = ApplicationBase.Container.Resolve<ILogManager>().CreateLogger(this.GetType().Name);

                    this.InitializeApplication();
                }

                mainFrame = (MainFrame)ApplicationBase.Container.Resolve<IMainFrame>();
                ((IViewPresenterBase)ApplicationBase.Container.Resolve<MainFramePresenter>()).Initialize(mainFrame);
                Container.Resolve<INavigationService>().RegisterRegionProvider(mainFrame);
                Window.Current.Content = mainFrame;
            }

            Window.Current.Activate();

            this.OnActivated(isFirstTimeActivate);
        }

        private void OnSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            var suspendingTask = this.OnSuspendingAsync();
            if (suspendingTask != null)
            {
                var deferral = suspendingEventArgs.SuspendingOperation.GetDeferral();

                suspendingTask.ContinueWith((t) =>
                {
                    if (this.Logger != null)
                    {
                        this.Logger.LogTask(t);
                    }

                    deferral.Complete();
                });
            }
        }
    }
}