// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;

    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The MainFrame interface.
    /// </summary>
    public interface IMainFrame : IView
    {
        /// <summary>
        /// Gets or sets a value indicating whether is top app bar open.
        /// </summary>
        bool IsTopAppBarOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is bottom app bar open.
        /// </summary>
        bool IsBottomAppBarOpen { get; set; }

        /// <summary>
        /// Set menu items.
        /// </summary>
        /// <param name="menuItems">
        /// The menu items.
        /// </param>
        void SetMenuItems(IEnumerable<MenuItemMetadata> menuItems);

        /// <summary>
        /// Set view commands.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        void SetViewCommands(IEnumerable<CommandMetadata> commands);

        /// <summary>
        /// The clear view commands.
        /// </summary>
        void ClearViewCommands();

        /// <summary>
        /// Set context commands.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        void SetContextCommands(IEnumerable<CommandMetadata> commands);

        /// <summary>
        /// Clear context commands.
        /// </summary>
        void ClearContextCommands();

        /// <summary>
        /// Show popup.
        /// </summary>
        /// <param name="popupRegion">
        /// The popup region.
        /// </param>
        /// <param name="injections">
        /// The injections arguments.
        /// </param>
        /// <typeparam name="TPopup">
        /// The type of popup view.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TPopup"/>.
        /// </returns>
        TPopup ShowPopup<TPopup>(PopupRegion popupRegion, params object[] injections) where TPopup : IPopupView;
    }

    /// <summary>
    /// The main frame.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "View/Interface implementation.")]
    public sealed partial class MainFrame : Page, IMainFrame, IMainFrameRegionProvider
    {
        private IDependencyResolverContainer container;
        private MainFramePresenter presenter;
        private ILogger logger;

        private IView currentView;

        private bool bottomToolWasOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainFrame"/> class.
        /// </summary>
        public MainFrame()
        {
            this.InitializeComponent();

            Debug.Assert(this.BottomAppBar != null, "this.BottomAppBar != null");
            this.BottomAppBar.Opened += (sender, o) =>
                {
                    this.BottomAppBarFakeBorder.Visibility = Visibility.Visible;
                };

            this.BottomAppBar.Closed += (sender, o) =>
                {
                    this.AppToolBarRightPopup.IsOpen = false;
                    this.AppToolBarLeftPopup.IsOpen = false;
                    this.BottomAppBarFakeBorder.Visibility = Visibility.Collapsed;
                };

            this.BottomAppBar.SizeChanged += (sender, args) =>
                {
                    this.BottomAppBarFakeBorder.Height = args.NewSize.Height;
                };

            this.SizeChanged += (sender, args) =>
                {
                    if (ApplicationView.Value == ApplicationViewState.Snapped)
                    {
                        this.FullViewGrid.Visibility = Visibility.Collapsed;
                        this.SnappedViewContentControl.Visibility = Visibility.Visible;

                        var control = this.SnappedViewContentControl.Content as Control;
                        if (control != null)
                        {
                            control.Focus(FocusState.Programmatic);
                        }

                        this.bottomToolWasOpen = this.BottomAppBar.IsOpen;
                    }
                    else
                    {
                        this.FullViewGrid.Visibility = Visibility.Visible;
                        this.SnappedViewContentControl.Visibility = Visibility.Collapsed;

                        var control = this.ContentControl.Content as Control;
                        if (control != null)
                        {
                            control.Focus(FocusState.Programmatic);
                        }
                    }

                    this.UpdateFullScreenPopupSize();
                    this.UpdateBottomAppBarVisibility();
                    this.UpdateTopAppBarVisibility();

                    if (ApplicationView.Value != ApplicationViewState.Snapped && this.bottomToolWasOpen)
                    {
                        this.BottomAppBar.IsOpen = bottomToolWasOpen;
                        this.bottomToolWasOpen = false;
                    }
                };
        }

        /// <inheritdoc />
        public bool IsTopAppBarOpen
        {
            get
            {
                return this.TopAppBar.IsOpen;
            }

            set
            {
                this.TopAppBar.IsOpen = value;
            }
        }

        /// <inheritdoc />
        public bool IsBottomAppBarOpen
        {
            get
            {
                return this.BottomAppBar.IsOpen;
            }

            set
            {
                this.BottomAppBar.IsOpen = value;
            }
        }

        /// <inheritdoc />
        public void SetMenuItems(IEnumerable<MenuItemMetadata> menuItems)
        {
            this.MainMenuItemsControl.ItemsSource = menuItems;

            this.UpdateTopAppBarVisibility();
        }

        /// <inheritdoc />
        public void SetViewCommands(IEnumerable<CommandMetadata> commands)
        {
            this.ViewButtonsItemsControl.ItemsSource = commands;
            this.UpdateBottomAppBar();
        }

        /// <inheritdoc />
        public void ClearViewCommands()
        {
            this.ViewButtonsItemsControl.ItemsSource = null;
            this.UpdateBottomAppBar();
        }

        /// <inheritdoc />
        public void SetContextCommands(IEnumerable<CommandMetadata> commands)
        {
            this.ContextButtonsItemsControl.ItemsSource = commands;
            this.UpdateBottomAppBar();
            if (this.BottomAppBar != null 
                && this.BottomAppBar.Visibility == Visibility.Visible 
                && !this.BottomAppBar.IsOpen
                && this.ContextButtonsItemsControl.Items != null
                && this.ContextButtonsItemsControl.Items.Count > 0)
            {
                this.BottomAppBar.IsOpen = true;
            }
        }

        /// <inheritdoc />
        public void ClearContextCommands()
        {
            this.ContextButtonsItemsControl.ItemsSource = null;
            this.UpdateBottomAppBar();
        }

        /// <inheritdoc />
        public TPopup ShowPopup<TPopup>(PopupRegion popupRegion, params object[] injections) where TPopup : IPopupView
        {
            TPopup popupView = this.container.Resolve<TPopup>(injections);
            var uiElement = (FrameworkElement)(object)popupView;
            this.ShowPopup(popupRegion, uiElement);
            return popupView;
        }
       
        /// <inheritdoc />
        public TPresenter GetPresenter<TPresenter>()
        {
            return (TPresenter)(object)this.presenter;
        }

        /// <inheritdoc />
        public void SetContent(MainFrameRegion region, object content)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Trying to set {0} to region {1}.", content, region);
            }

            switch (region)
            {
                case MainFrameRegion.Content:
                    this.SetContentRegion(content);
                    break;

                case MainFrameRegion.Right:
                    this.SetRightRegion(content);
                    break;

                case MainFrameRegion.BottomAppBarRightZone:
                    this.SetBottomAppBarRightZoneRegion(content);
                    break;

                case MainFrameRegion.Background:
                    this.SetBackgroundRegion(content);
                    break;

                case MainFrameRegion.Links:
                    this.SetLinksRegion(content);
                    break;

                case MainFrameRegion.SnappedView:
                    this.SetSnappedRegion(content);
                    break;

                case MainFrameRegion.TopAppBarRightZone:
                    this.SetTopAppBarRightZoneRegion(content);
                    break;

                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Region {0} is not supported.", region));
            }
        }

        /// <inheritdoc />
        public void SetContent<TView>(MainFrameRegion region, params object[] injections)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Trying to set {0} to region {1}.", typeof(TView), region);
            }

            object content = this.container.Resolve<TView>(injections);

            this.SetContent(region, content);
        }

        /// <inheritdoc />
        public void SetVisibility(MainFrameRegion region, bool isVisible)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Trying to set visibility '{0}' to region {1}.", isVisible, region);
            }

            switch (region)
            {
                case MainFrameRegion.Content:
                    this.ContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.Right:
                    this.RightRegionContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.BottomAppBarRightZone:
                    this.BottomAppBarRightZoneRegionContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.Background:
                    this.BackgroundContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.Links:
                    this.LinksContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.SnappedView:
                    this.SnappedViewContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case MainFrameRegion.TopAppBarRightZone:
                    this.TopAppBarRightZoneRegionContentControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;

                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Region {0} is not supported.", region));
            }
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
        }

        private void ShowPopup(PopupRegion region, FrameworkElement content)
        {
            switch (region)
            {
                case PopupRegion.AppToolBarRight:
                    this.DisposePopupContent(this.AppToolBarRightPopup);
                    this.AppToolBarRightPopup.VerticalOffset = 0;
                    this.AppToolBarRightPopup.Child = content;
                    this.AppToolBarRightPopup.Width = content.Width;
                    this.AppToolBarRightPopup.Height = content.Height;
                    this.AppToolBarRightPopup.IsOpen = true;
                    break;
                case PopupRegion.AppToolBarLeft:
                    this.DisposePopupContent(this.AppToolBarLeftPopup);
                    this.AppToolBarLeftPopup.VerticalOffset = 0;
                    this.AppToolBarLeftPopup.Child = content;
                    this.AppToolBarLeftPopup.Width = content.Width;
                    this.AppToolBarLeftPopup.Height = content.Height;
                    this.AppToolBarLeftPopup.IsOpen = true;
                    break;
                case PopupRegion.Full:
                    this.DisposePopupContent(this.FullScreenPopup);
                    this.FullScreenPopup.Child = content;
                    this.UpdateFullScreenPopupSize();
                    this.FullScreenPopup.IsOpen = true;
                    this.UpdateBottomAppBarVisibility();
                    this.UpdateTopAppBarVisibility();
                    ((Storyboard)this.Resources["ActivateFullScreenPopup"]).Begin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("region");
            }
        }

        private void UpdateFullScreenPopupSize()
        {
            this.FullScreenPopup.Width = Window.Current.Bounds.Width;
            this.FullScreenPopup.Height = Window.Current.Bounds.Height;
            var frameworkElement = this.FullScreenPopup.Child as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Height = this.FullScreenPopup.Height;
                frameworkElement.Width = this.FullScreenPopup.Width;
            }
        }

        private void OnIsDataLoadingChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var view = this.currentView;
            if (view is IPageView)
            {
                var viewPresenter = view.GetPresenter<IPagePresenterBase>();
                this.ProgressRing.IsActive = viewPresenter.IsDataLoading;
                if (!this.ProgressRing.IsActive)
                {
                    ((Storyboard)this.Resources["ActivateContent"]).Begin();
                }
            }
        }

        private void MainMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItemMetadata = e.ClickedItem as MenuItemMetadata;
            if (menuItemMetadata != null)
            {
                this.presenter.NavigateTo(menuItemMetadata);
            }
            else
            {
                this.logger.Error("Could not find MenuItemMetadata in ClickedItem.");
            }

            if (this.TopAppBar != null)
            {
                this.TopAppBar.IsOpen = false;
            }
        }

        private void PopupViewClosed(object sender, object e)
        {
            var popup = sender as Popup;
            if (popup != null)
            {
                this.DisposePopupContent(popup);
            }
        }

        private void DisposePopupContent(Popup popup)
        {
            UIElement content = popup.Child;
            popup.Child = null;

            var popupViewBase = content as PopupViewBase;
            if (popupViewBase != null)
            {
                try
                {
                    popupViewBase.GetPresenter<BindingModelBase>().DisposeIfDisposable();
                }
                catch (Exception exp)
                {
                    this.logger.Error(exp, "Exception while tried to dispose presenter for popup view base.");
                }
            }

            try
            {
                content.DisposeIfDisposable();
            }
            catch (Exception exp)
            {
                this.logger.Error(exp, "Exception while tried to dispose content of popup view base.");
            }
        }

        private void FullScreenPopupViewClosed(object sender, object e)
        {
            this.PopupViewClosed(sender, e);
            this.UpdateBottomAppBarVisibility();
            this.UpdateTopAppBarVisibility();
        }

        private void SetContentRegion(object content)
        {
            if (this.currentView != null)
            {
                this.currentView.GetPresenter<BindingModelBase>().Unsubscribe("IsDataLoading", this.OnIsDataLoadingChanged);

                try
                {
                    this.currentView.GetPresenter<BindingModelBase>().DisposeIfDisposable();
                }
                catch (Exception exp)
                {
                    this.logger.Error(exp, "Exception while tried to dispose presenter for curren view.");
                }

                try
                {
                    this.currentView.DisposeIfDisposable();
                }
                catch (Exception exp)
                {
                    this.logger.Error(exp, "Exception while tried to dispose current view.");
                }
                
                this.currentView = null;
            }

            this.ClearViewCommands();
            this.ClearContextCommands();

            this.ContentControl.Content = null;
            this.TitleTextBox.ClearValue(TextBlock.TextProperty);
            this.SubtitleTextBox.ClearValue(TextBlock.TextProperty);
            this.TitleGrid.ClearValue(UIElement.VisibilityProperty);
            this.StoreLogoImage.ClearValue(UIElement.VisibilityProperty);

            this.currentView = content as IView;

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

            var dataPageView = this.currentView as IPageView;
            if (dataPageView != null)
            {
                this.ProgressRing.IsActive = true;
                this.ContentControl.Opacity = 0;
                this.currentView.GetPresenter<BindingModelBase>().Subscribe("IsDataLoading", this.OnIsDataLoadingChanged);
            }

            this.ContentControl.Content = this.currentView;
        }

        private void SetRightRegion(object content)
        {
            this.RightRegionContentControl.Content = content;
        }

        private void SetBackgroundRegion(object content)
        {
            this.BackgroundContentControl.Content = content;
        }

        private void SetLinksRegion(object content)
        {
            this.LinksContentControl.Content = content;
        }

        private void SetSnappedRegion(object content)
        {
            this.SnappedViewContentControl.Content = content;
        }

        private void SetTopAppBarRightZoneRegion(object content)
        {
            this.TopAppBarRightZoneRegionContentControl.Content = content;
        }

        private void SetBottomAppBarRightZoneRegion(object content)
        {
            this.BottomAppBarRightZoneRegionContentControl.Content = content;
            if (content == null)
            {
                this.ContextButtonsItemsControl.HorizontalAlignment = HorizontalAlignment.Right;
                this.BottomAppBar.IsSticky = false;
            }
            else
            {
                this.ContextButtonsItemsControl.HorizontalAlignment = HorizontalAlignment.Left;
                this.BottomAppBar.IsSticky = true;
            }

            this.UpdateBottomAppBar();
        }

        private void UpdateBottomAppBar()
        {
            if (this.ContextButtonsItemsControl.Items != null && this.ContextButtonsItemsControl.Items.Count > 0
                && this.ViewButtonsItemsControl.Items != null && this.ViewButtonsItemsControl.Items.Count > 0
                && this.BottomAppBarRightZoneRegionContentControl.Content != null)
            {
                this.AppToolbarSeparator.Visibility = Visibility.Visible;
            }
            else
            {
                this.AppToolbarSeparator.Visibility = Visibility.Collapsed;
            }

            this.UpdateBottomAppBarVisibility();
        }

        private void UpdateBottomAppBarVisibility()
        {
            bool isVisible = (this.ContextButtonsItemsControl.Items != null && this.ContextButtonsItemsControl.Items.Count > 0)
                             || (this.ViewButtonsItemsControl.Items != null && this.ViewButtonsItemsControl.Items.Count > 0)
                             || this.BottomAppBarRightZoneRegionContentControl.Content != null;
            this.UpdateToolBarVisibility(this.BottomAppBar, isVisible);
        }

        private void UpdateTopAppBarVisibility()
        {
            bool isVisible = this.MainMenuItemsControl.Items != null && this.MainMenuItemsControl.Items.Count > 0;
            this.UpdateToolBarVisibility(this.TopAppBar, isVisible);
        }

        private void UpdateToolBarVisibility(AppBar appBar, bool isLogicalVisible)
        {
            if (appBar != null)
            {
                var currentVisibility = appBar.Visibility == Visibility.Visible && appBar.IsOpen;

                var isVisible = ApplicationView.Value != ApplicationViewState.Snapped && !this.FullScreenPopup.IsOpen && isLogicalVisible;

                appBar.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

                if (!currentVisibility)
                {
                    appBar.IsOpen = false;
                }
            }
        }

        private void TopAppBarOpened(object sender, object e)
        {
            // This is stupid way to fix problem with that when we show Main Menu - we can see that one of the items will have On Hover state.
            var itemsSource = this.MainMenuItemsControl.ItemsSource;
            this.MainMenuItemsControl.ItemsSource = null;
            this.MainMenuItemsControl.ItemsSource = itemsSource;
        }
    }
}
