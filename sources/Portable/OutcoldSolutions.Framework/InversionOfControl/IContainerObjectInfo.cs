// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The RegisteredObject interface.
    /// </summary>
    public interface IContainerObjectInfo
    {
        /// <summary>
        /// The implementation.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IContainerObjectInfo"/>.
        /// </returns>
        IContainerObjectInfo For(Type type);

        /// <summary>
        /// The as singleton.
        /// </summary>
        void AsSingleton();

        /// <summary>
        /// The as singleton.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        void AsSingleton(object instance);
    }
}