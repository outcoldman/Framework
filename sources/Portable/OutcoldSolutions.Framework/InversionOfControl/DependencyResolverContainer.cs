// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// The dependency resolver container.
    /// </summary>
    public class DependencyResolverContainer : IDependencyResolverContainerEx
    {
        private readonly Dictionary<Type, IContainerObjectInfo> registeredObjects = new Dictionary<Type, IContainerObjectInfo>();
        private readonly object registractionContextLocker = new object();
        private IRegistrationContext currentRegistrationContext;

        /// <inheritdoc />
        public IRegistrationContext GetRegistrationContext()
        {
            bool monitorTaken;
            if ((monitorTaken = Monitor.TryEnter(this.registractionContextLocker)) && this.currentRegistrationContext == null)
            {
                return this.currentRegistrationContext = new RegistrationContext(this);
            }

            if (monitorTaken)
            {
                Monitor.Exit(this.registractionContextLocker);
            }

            throw new NotSupportedException(FrameworkResources.ErrMsg_PreviosCreatedContextIsNotDisposed);
        }

        /// <inheritdoc />
        public bool IsRegistered(Type type)
        {
            return this.registeredObjects.ContainsKey(type);
        }

        void IDependencyResolverContainerEx.RemoveRegistrationContext(IRegistrationContext registrationContext)
        {
            if (registrationContext == null)
            {
                throw new ArgumentNullException("registrationContext");
            }

            if (this.currentRegistrationContext != registrationContext)
            {
                throw new ArgumentException("registrationContext");
            }

            this.currentRegistrationContext = null;
            Monitor.Exit(this.registractionContextLocker);
        }

        void IDependencyResolverContainerEx.Add(Type type, ContainerObjectInfo objectInfo, IRegistrationContext registrationContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (objectInfo == null)
            {
                throw new ArgumentNullException("objectInfo");
            }

            if (this.currentRegistrationContext == null || this.currentRegistrationContext != registrationContext)
            {
                throw new NotSupportedException(FrameworkResources.ErrMsg_ParentRegistrationContextIsDisposed);
            }

            this.registeredObjects.Add(type, objectInfo);
        }
    }
}