// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;

    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The MainFrame interface.
    /// </summary>
    public interface IMainFrame : IView
    {
    }

    /// <summary>
    /// The main frame.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "View/Interface implementation.")]
    public sealed partial class MainFrame : Page, IMainFrame, IApplicationToolbar, IViewRegionProvider
    {
        private IDependencyResolverContainer container;
        private MainFramePresenter presenter;
        private ILogger logger;

        private IView currentView;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainFrame"/> class.
        /// </summary>
        public MainFrame()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc />
        public void SetMenuItems(IEnumerable<MenuItemMetadata> menuItems)
        {
            this.MainMenuItemsControl.ItemsSource = menuItems;
        }

        /// <inheritdoc />
        public void SetViewCommands(IEnumerable<CommandMetadata> commands)
        {
        }

        /// <inheritdoc />
        public void ClearViewCommands()
        {
        }

        /// <inheritdoc />
        public void SetContextCommands(IEnumerable<CommandMetadata> commands)
        {
        }

        /// <inheritdoc />
        public void ClearContextCommands()
        {
        }

        /// <inheritdoc />
        public void ShowPopup<TPopup>(params object[] arguments) where TPopup : IPopupView
        {
        }

        /// <inheritdoc />
        public TPresenter GetPresenter<TPresenter>()
        {
            return (TPresenter)(object)this.presenter;
        }

        /// <inheritdoc />
        public async void Show(IView view)
        {
            await this.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, 
                () =>
                {
                    this.currentView = view;

                    this.ClearViewCommands();
                    this.ClearContextCommands();

                    this.ContentControl.Content = null;
                    this.TitleTextBox.ClearValue(TextBlock.TextProperty);
                    this.SubtitleTextBox.ClearValue(TextBlock.TextProperty);
                    this.TitleGrid.ClearValue(UIElement.VisibilityProperty);
                    this.StoreLogoImage.ClearValue(UIElement.VisibilityProperty);
                    
                    var pageView = this.currentView as IPageView;
                    if (pageView != null)
                    {
                        this.TitleTextBox.SetBinding(
                            TextBlock.TextProperty,
                            new Binding()
                                {
                                    Source = this.currentView, 
                                    Mode = BindingMode.OneWay,
                                    Path = new PropertyPath(PropertyNameExtractor.GetPropertyName(() => pageView.Title))
                                });

                        this.SubtitleTextBox.SetBinding(
                            TextBlock.TextProperty,
                            new Binding()
                                {
                                    Source = this.currentView,
                                    Mode = BindingMode.OneWay,
                                    Path = new PropertyPath(PropertyNameExtractor.GetPropertyName(() => pageView.Subtitle))
                                });

                        this.TitleGrid.SetBinding(
                            UIElement.VisibilityProperty,
                            new Binding()
                                {
                                    Source = this.currentView,
                                    Mode = BindingMode.OneWay,
                                    Path = new PropertyPath(PropertyNameExtractor.GetPropertyName(() => pageView.IsTitleVisible)),
                                    Converter = (IValueConverter)Application.Current.Resources["BooleanToVisibilityConverter"]
                                });

                        this.StoreLogoImage.SetBinding(
                            UIElement.VisibilityProperty,
                            new Binding()
                            {
                                Source = this.currentView,
                                Mode = BindingMode.OneWay,
                                Path = new PropertyPath(PropertyNameExtractor.GetPropertyName(() => pageView.IsStoreLogoVisible)),
                                Converter = (IValueConverter)Application.Current.Resources["BooleanToVisibilityConverter"]
                            });
                    }

                    var dataPageView = this.currentView as IDataPageView;
                    if (dataPageView != null)
                    {
                        this.ProgressRing.IsActive = true;
                        this.ContentControl.Opacity = 0;
                        view.GetPresenter<PresenterBase>().Subscribe("IsDataLoading", OnIsDataLoadingChanged);
                    }

                    this.ContentControl.Content = this.currentView;
                });
        }

        [Inject]
        internal void Initialize(
            IDependencyResolverContainer containerObject,
            ILogManager logManager,
            MainFramePresenter presenterObject)
        {
            this.container = containerObject;
            this.presenter = presenterObject;
            this.logger = logManager.CreateLogger("MainFrame");
            this.DataContext = this.presenter;

            ((IViewPresenterBase)this.presenter).Initialize(this);
        }

        private void OnIsDataLoadingChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var view = this.currentView;
            if (view is IDataPageView)
            {
                var viewPresenter = view.GetPresenter<IDataPagePresenterBase>();
                this.ProgressRing.IsActive = viewPresenter.IsDataLoading;
                if (!this.ProgressRing.IsActive)
                {
                    ((Storyboard)this.Resources["ActivateContent"]).Begin();
                }
            }
        }

        private void MainMenuItemClick(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                var menuItemMetadata = button.DataContext as MenuItemMetadata;
                if (menuItemMetadata != null)
                {
                    this.presenter.NavigateTo(menuItemMetadata);
                }
                else
                {
                    this.logger.Error("Could not find MenuItemMetadata in DataContext.");
                }
            }
            else
            {
                this.logger.Error("Could not cast sender to FrameworkElement.");
            }

            if (this.TopAppBar != null)
            {
                this.TopAppBar.IsOpen = false;
            }
        }
    }
}
