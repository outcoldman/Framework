// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test
{
    using System.Collections.Generic;

    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presentation.Test.BindingModel;
    using OutcoldSolutions.Presentation.Test.Diagnostics;
    using OutcoldSolutions.Presentation.Test.Presenters;
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presenters;
    using OutcoldSolutions.Views;

    /// <summary>
    /// The application base.
    /// </summary>
    public partial class App : ApplicationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void InitializeApplication()
        {
            using (var registration = Container.Registration())
            {
                registration.Register<IDebugConsole>().AsSingleton<DebugConsole>();

                registration.Register<IStartPageView>()
                    .InjectionRule<PresenterBase, StartPageViewPresenter>()
                    .AsSingleton<StartPageView>();
                registration.Register<StartPageViewPresenter>().AsSingleton();

                registration.Register<ITestDataPageView>()
                    .InjectionRule<PresenterBase, TestDataPageViewPresenter>()
                    .AsSingleton<TestDataPageView>();
                registration.Register<TestDataPageViewPresenter>().AsSingleton();
                registration.Register<TestDataPageViewBindingModel>().AsSingleton();
            }
        }

        protected override void OnActivated()
        {
            Container.Resolve<IApplicationToolbar>().SetMenuItems(new List<MenuItemMetadata>()
                                                                      {
                                                                          MenuItemMetadata.FromViewType<IStartPageView>("Start view"),
                                                                          MenuItemMetadata.FromViewType<ITestDataPageView>("Test Data Page")
                                                                      });
            Container.Resolve<INavigationService>().NavigateTo<IStartPageView>();
        }
    }
}