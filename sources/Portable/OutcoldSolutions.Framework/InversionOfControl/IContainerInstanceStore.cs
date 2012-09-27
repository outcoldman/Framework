// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// Contains methods which allows to add / remove container instances.
    /// </summary>
    internal interface IContainerInstanceStore 
    {
        /// <summary>
        /// Remove reference to <paramref name="registrationContext"/>.
        /// </summary>
        /// <param name="registrationContext">The registration context.</param>
        void OnRegistrationContextDisposing(IRegistrationContext registrationContext);

        /// <summary>
        /// Add new container instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">Object info for current type.</param>
        /// <param name="registrationContext">Registration context, which was asked to register this type.</param>
        void Add(Type type, ContainerInstance instance, IRegistrationContext registrationContext);

        /// <summary>
        /// Get container instance by type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The container instance.</returns>
        ContainerInstance Get(Type type);
    }
}