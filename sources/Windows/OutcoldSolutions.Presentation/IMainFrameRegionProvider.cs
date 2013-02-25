// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using OutcoldSolutions.Views;

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
    }
}