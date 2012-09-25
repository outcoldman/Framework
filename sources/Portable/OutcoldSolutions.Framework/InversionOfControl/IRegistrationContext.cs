// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The RegistrationContext interface.
    /// </summary>
    public interface IRegistrationContext : IDisposable
    {
        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="typeImplementation">
        /// The type implementation.
        /// </param>
        /// <returns>
        /// The <see cref="IContainerObjectInfo"/>.
        /// </returns>
        IContainerObjectInfo Register(Type typeImplementation);
    }
}