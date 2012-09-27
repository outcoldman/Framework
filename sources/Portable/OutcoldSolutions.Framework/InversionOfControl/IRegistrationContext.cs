// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The registration context for <see cref="IDependencyResolverContainer"/>.
    /// </summary>
    public interface IRegistrationContext : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Start registration process for type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type implementation. 
        /// </param>
        /// <returns>
        /// Instance of <see cref="IContainerInstance"/>, which allows you to specify registration settings 
        /// for <paramref name="type"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> is null.</exception>
        IContainerInstance Register(Type type);
    }
}