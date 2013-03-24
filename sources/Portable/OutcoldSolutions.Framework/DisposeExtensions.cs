//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// <see cref="IDisposable"/> extensions.
    /// </summary>
    public static class DisposeExtensions
    {
        /// <summary>
        /// Dispose object if it implements <see cref="IDisposable"/> interface.
        /// </summary>
        /// <param name="object">
        /// The object.
        /// </param>
        public static void DisposeIfDisposable(this object @object)
        {
            var disposable = @object as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
