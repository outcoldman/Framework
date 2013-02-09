// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The task ex.
    /// </summary>
    public static class TaskEx
    {
        /// <summary>
        /// Continue when all tasks are completed. This method ignores <see cref="TaskCanceledException"/>.
        /// </summary>
        /// <param name="tasks">
        /// The tasks.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<Task[]> WhenAllSafe(params Task[] tasks)
        {
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (TaskCanceledException)
            {
            }

            return tasks;
        }

        /// <summary>
        /// Continue when all tasks are completed. This method ignores <see cref="TaskCanceledException"/>.
        /// </summary>
        /// <param name="tasks">
        /// The tasks.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="tasks"/> is null.
        /// </exception>
        public static async Task<Task[]> WhenAllSafe(IEnumerable<Task> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return await WhenAllSafe(tasks.ToArray());
        }

        /// <summary>
        /// Wait all tasks. This method ignores <see cref="TaskCanceledException"/>.
        /// </summary>
        /// <param name="tasks">
        /// The tasks.
        /// </param>
        public static void WaitAllSafe(params Task[] tasks)
        {
            try
            {
                Task.WaitAll(tasks);
            }
            catch (TaskCanceledException)
            {
            }
        }

        /// <summary>
        /// Wait all tasks. This method ignores <see cref="TaskCanceledException"/>.
        /// </summary>
        /// <param name="tasks">
        /// The tasks.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="tasks"/> is null.
        /// </exception>
        public static void WaitAllSafe(IEnumerable<Task> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            WaitAllSafe(tasks.ToArray());
        }
    }
}