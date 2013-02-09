// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
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

        /// <summary>
        /// Start registration process for type <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">
        /// The type implementation. 
        /// </typeparam>
        /// <returns>
        /// Instance of <see cref="IContainerInstance"/>, which allows you to specify registration settings 
        /// for <typeparamref name="TType"/>.
        /// </returns>
        IContainerInstance Register<TType>();
    }
}