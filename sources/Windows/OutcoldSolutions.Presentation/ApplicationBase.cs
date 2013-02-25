﻿// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Threading.Tasks;

    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;
    using OutcoldSolutions.Views;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Core;
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
        /// The activated event.
        /// </summary>
        protected abstract void Activated();

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
            MainFrame mainFrame = Window.Current.Content as MainFrame;
            if (mainFrame == null)
            {
                if (Container == null)
                {
                    Container = new DependencyResolverContainer();

                    using (var registration = Container.Registration())
                    {
                        registration.Register<ILogManager>().AsSingleton<LogManager>();
                        registration.Register<INavigationService>().AsSingleton<NavigationService>();
                        registration.Register<IDispatcher>().AsSingleton(new DispatcherContainer(CoreWindow.GetForCurrentThread().Dispatcher));

                        registration.Register<IMainFrame>()
                                    .And<IApplicationToolbar>()
                                    .InjectionRule<PresenterBase, MainFramePresenter>()
                                    .AsSingleton<MainFrame>();
                        registration.Register<MainFramePresenter>().AsSingleton();
                    }

                    this.InitializeApplication();
                }

                mainFrame = (MainFrame)Container.Resolve<IMainFrame>();
                Container.Resolve<INavigationService>().RegisterRegionProvider(mainFrame);
                Window.Current.Content = mainFrame;
            }

            Window.Current.Activate();

            this.Activated();
        }

        private void OnSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            var deferral = suspendingEventArgs.SuspendingOperation.GetDeferral();

            var suspendingTask = this.OnSuspendingAsync();
            if (suspendingTask != null)
            {
                suspendingTask.ContinueWith((t) =>
                {
                    // We need to log task
                    deferral.Complete();
                });
            }
            else
            {
                deferral.Complete();
            }
        }
    }
}