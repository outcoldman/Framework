// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System;
    using System.Diagnostics;

    using OutcoldSolutions.Controls;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The data page view base.
    /// </summary>
    public class DataPageViewBase : PageViewBase, IDataPageView
    {
        private const string HorizontalScrollOffset = "ListView_HorizontalScrollOffset";
        private const string VerticalScrollOffset = "ListView_VerticalScrollOffset";

        private ItemsControl trackingItemsControl;
        private Storyboard trackingListStoryboard;

        /// <summary>
        /// On navigating from.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public override void OnNavigatingFrom(NavigatingFromEventArgs eventArgs)
        {
            base.OnNavigatingFrom(eventArgs);

            if (this.trackingItemsControl != null)
            {
                eventArgs.State[HorizontalScrollOffset] = 
                    this.trackingItemsControl.GetScrollViewerHorizontalOffset();
                eventArgs.State[VerticalScrollOffset] =
                    this.trackingItemsControl.GetScrollViewerVerticalOffset();

                this.trackingItemsControl.Opacity = 0;
            }
        }

        /// <summary>
        /// On data loading.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnDataLoading(NavigatedToEventArgs eventArgs)
        {
            if (this.trackingItemsControl != null)
            {
                this.trackingItemsControl.ScrollToHorizontalZero();
                this.trackingItemsControl.ScrollToVerticalZero();
            }
        }

        /// <summary>
        /// On unfreeze.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnUnfreeze(NavigatedToEventArgs eventArgs)
        {
        }

        /// <summary>
        /// On data loaded.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnDataLoaded(NavigatedToEventArgs eventArgs)
        {
            if (this.trackingItemsControl != null)
            {
                if (eventArgs.IsNavigationBack)
                {
                    object offset;
                    if (eventArgs.State.TryGetValue(HorizontalScrollOffset, out offset))
                    {
                        this.trackingItemsControl.ScrollToHorizontalOffset((double)offset);
                    }

                    if (eventArgs.State.TryGetValue(VerticalScrollOffset, out offset))
                    {
                        this.trackingItemsControl.ScrollToVerticalOffset((double)offset);
                    }
                }

                this.trackingListStoryboard.Begin();
            }
        }

        /// <summary>
        /// Track list view base.
        /// </summary>
        /// <param name="itemsControl">
        /// The list view base.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="itemsControl"/> is null.
        /// </exception>
        protected void TrackItemsControl(ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            Debug.Assert(this.trackingItemsControl == null, "this.trackingItemsControl == null. Only one list view tracking supported.");
            this.trackingItemsControl = itemsControl;
            if (this.trackingItemsControl.Transitions != null)
            {
                this.trackingItemsControl.Transitions.Clear();
            }

            this.trackingItemsControl.Opacity = 0;

            this.trackingListStoryboard = new Storyboard();
            DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames, this.trackingItemsControl);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames, "Opacity");
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), Value = 0 });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)), Value = 0 });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300)), Value = 1 });
            this.trackingListStoryboard.Children.Add(doubleAnimationUsingKeyFrames);
            this.Resources.Add("TrackingListStoryboard", this.trackingListStoryboard);
        }
    }
}