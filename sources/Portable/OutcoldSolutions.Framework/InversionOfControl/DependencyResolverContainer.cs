// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// The dependency resolver container.
    /// </summary>
    public class DependencyResolverContainer : IDependencyResolverContainerEx
    {
        private readonly Dictionary<Type, ContainerObjectInfo> registeredObjects = new Dictionary<Type, ContainerObjectInfo>();
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
            lock (this.registractionContextLocker)
            {
                return this.registeredObjects.ContainsKey(type);
            }
        }

        /// <inheritdoc />
        public object Resolve(Type type, object[] arguments = null)
        {
            ContainerObjectInfo objectInfo;
            return ((IDependencyResolverContainerEx)this).ResolveAndGet(type, arguments, out objectInfo);
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

        ContainerObjectInfo IDependencyResolverContainerEx.Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ContainerObjectInfo objectInfo;
            return this.registeredObjects.TryGetValue(type, out objectInfo) ? objectInfo : null;
        }

        object IDependencyResolverContainerEx.ResolveAndGet(Type type, object[] arguments, out ContainerObjectInfo objectInfo)
        {
            lock (this.registractionContextLocker)
            {
                if (this.registeredObjects.TryGetValue(type, out objectInfo))
                {
                    return objectInfo.Resolve(arguments);
                }

                throw new ArgumentOutOfRangeException(
                    "type",
                    string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_TypeIsNotRegistered, type));
            }
        }
    }
}