// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Threading.Tasks;

    using Windows.UI.Core;

    /// <summary>
    /// The Dispatcher interface.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Run async.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task RunAsync(Action action);

        /// <summary>
        /// Run async.
        /// </summary>
        /// <param name="priority">
        /// The priority.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task RunAsync(CoreDispatcherPriority priority, Action action);
    }
}