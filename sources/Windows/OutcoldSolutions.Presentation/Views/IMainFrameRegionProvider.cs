// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    /// <summary>
    /// The IMainFrameRegionProvider interface.
    /// </summary>
    public interface IMainFrameRegionProvider
    {
        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        void SetContent(MainFrameRegion region, object content);

        /// <summary>
        /// Set visibility for region.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="isVisible">
        /// The is visible.
        /// </param>
        void SetVisibility(MainFrameRegion region, bool isVisible);
    }
}