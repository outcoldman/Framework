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

        public static async Task<Task[]> WhenAllSafe(IEnumerable<Task> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return await WhenAllSafe(tasks.ToArray());
        }

        public static Task[] WaitAllSafe(params Task[] tasks)
        {
            try
            {
                Task.WaitAll(tasks);
            }
            catch (TaskCanceledException)
            {
            }

            return tasks;
        }

        public static Task[] WaitAllSafe(IEnumerable<Task> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return WaitAllSafe(tasks.ToArray());
        }
    }
}