// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// Internal addition methods for <see cref="IDependencyResolverContainer"/>.
    /// </summary>
    internal interface IDependencyResolverContainerEx : IDependencyResolverContainer
    {
        /// <summary>
        /// Remove reference to <paramref name="registrationContext"/>.
        /// </summary>
        /// <param name="registrationContext">The registration context.</param>
        void RemoveRegistrationContext(IRegistrationContext registrationContext);

        /// <summary>
        /// Add new registered information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectInfo">Object info for current type.</param>
        /// <param name="registrationContext">Registration context, which was asked to register this type.</param>
        void Add(Type type, ContainerObjectInfo objectInfo, IRegistrationContext registrationContext);

        ContainerObjectInfo Get(Type type);

        object ResolveAndGet(Type type, object[] arguments, out ContainerObjectInfo objectInfo);
    }
}