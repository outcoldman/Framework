// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The DependencyResolverContainer interface.
    /// </summary>
    public interface IDependencyResolverContainer
    {
        /// <summary>
        /// The get registration context.
        /// </summary>
        /// <returns>
        /// The <see cref="IRegistrationContext"/>.
        /// </returns>
        IRegistrationContext GetRegistrationContext();

        /// <summary>
        /// The is registered.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsRegistered(Type type);
    }
}