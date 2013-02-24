// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    /// <summary>
    /// The DataPageView interface.
    /// </summary>
    public interface IDataPageView : IPageView
    {
        /// <summary>
        /// On data loading.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnDataLoading(NavigatedToEventArgs eventArgs);

        /// <summary>
        /// On unfreeze.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnUnfreeze(NavigatedToEventArgs eventArgs);

        /// <summary>
        /// On data loaded.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnDataLoaded(NavigatedToEventArgs eventArgs);
    }
}