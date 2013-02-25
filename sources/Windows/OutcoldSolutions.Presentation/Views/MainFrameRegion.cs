// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    /// <summary>
    /// The main frame region.
    /// </summary>
    public enum MainFrameRegion
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The content region. At the center of page.
        /// </summary>
        Content = 1,

        /// <summary>
        /// The region in bottom right corner of App Bar ("Player Zone").
        /// </summary>
        BottomAppBarRightZone = 2,

        /// <summary>
        /// Right column ("Ads zone").
        /// </summary>
        Right = 3,

        /// <summary>
        /// We give whole area to this region. It can be used to place something in background.
        /// </summary>
        Background = 4
    }
}