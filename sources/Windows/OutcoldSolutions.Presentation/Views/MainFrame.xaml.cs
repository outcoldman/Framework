﻿// --------------------------------------------------------------------------------------------------------------------
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

    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Presenters;

    using Windows.UI.ViewManagement;
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
    public sealed partial class MainFrame : Page, IMainFrame, IApplicationToolbar, IMainFrameRegionProvider
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

            Debug.Assert(this.BottomAppBar != null, "this.BottomAppBar != null");
            this.BottomAppBar.Opened += (sender, o) =>
                {
                    this.BottomAppBarFakeBorder.Visibility = Visibility.Visible;
                };

            this.BottomAppBar.Closed += (sender, o) =>
                {
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
                    }
                    else
                    {
                        this.FullViewGrid.Visibility = Visibility.Visible;
                        this.SnappedViewContentControl.Visibility = Visibility.Collapsed;
                    }
                };
        }

        /// <inheritdoc />
        public void SetMenuItems(IEnumerable<MenuItemMetadata> menuItems)
        {
            this.MainMenuItemsControl.ItemsSource = menuItems;
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
        }

        /// <inheritdoc />
        public void ClearContextCommands()
        {
            this.ContextButtonsItemsControl.ItemsSource = null;
            this.UpdateBottomAppBar();
        }

        /// <inheritdoc />
        public void ShowPopup<TPopup>(params object[] arguments) where TPopup : IPopupView
        {
            TPopup popupView = this.container.Resolve<TPopup>(arguments);
            var uiElement = (UIElement)(object)popupView;
            this.PopupView.Child = uiElement;
            this.PopupView.IsOpen = true;
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

        private void MainMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void PopupViewClosed(object sender, object e)
        {
            this.PopupView.Child = null;
        }

        private void SetContentRegion(object content)
        {
            if (this.currentView != null)
            {
                this.currentView.GetPresenter<PresenterBase>().Unsubscribe("IsDataLoading", this.OnIsDataLoadingChanged);
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

            var dataPageView = this.currentView as IDataPageView;
            if (dataPageView != null)
            {
                this.ProgressRing.IsActive = true;
                this.ContentControl.Opacity = 0;
                this.currentView.GetPresenter<PresenterBase>().Subscribe("IsDataLoading", this.OnIsDataLoadingChanged);
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

        private void SetBottomAppBarRightZoneRegion(object content)
        {
            this.BottomAppBarRightZoneRegionContentControl.Content = content;
            if (content == null)
            {
                this.ContextButtonsItemsControl.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.ContextButtonsItemsControl.HorizontalAlignment = HorizontalAlignment.Left;
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

            if (this.BottomAppBar != null)
            {
                this.BottomAppBar.Visibility = ((this.ContextButtonsItemsControl.Items != null && this.ContextButtonsItemsControl.Items.Count > 0)
                                                || (this.ViewButtonsItemsControl.Items != null && this.ViewButtonsItemsControl.Items.Count > 0)
                                                || this.BottomAppBarRightZoneRegionContentControl.Content != null)
                                                   ? Visibility.Visible
                                                   : Visibility.Collapsed;
            }
        }
    }
}
