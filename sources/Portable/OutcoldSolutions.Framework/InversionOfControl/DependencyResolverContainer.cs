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
    /// Implementation of <see cref="IDependencyResolverContainer"/>.
    /// </summary>
    public class DependencyResolverContainer : IContainerInstanceStore, IDependencyResolverContainer
    {
        private readonly Dictionary<Type, ContainerInstance> registeredObjects = new Dictionary<Type, ContainerInstance>();
        private readonly object registractionContextLocker = new object();
        private IRegistrationContext currentRegistrationContext;

        /// <inheritdoc />
        public IRegistrationContext GetRegistration()
        {
            bool monitorTaken;
            if ((monitorTaken = Monitor.TryEnter(this.registractionContextLocker)) && this.currentRegistrationContext == null)
            {
                return this.currentRegistrationContext = new RegistrationContext(this, this);
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
        public bool IsRegistered<TType>()
        {
            return this.IsRegistered(typeof(TType));
        }

        /// <inheritdoc />
        public object Resolve(Type type, params object[] arguments)
        {
            lock (this.registractionContextLocker)
            {
                ContainerInstance instance;
                if (this.registeredObjects.TryGetValue(type, out instance))
                {
                    return instance.Resolve(arguments);
                }

                throw new ArgumentOutOfRangeException(
                    "type",
                    string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_TypeIsNotRegistered, type));
            }
        }

        /// <inheritdoc />
        public TType Resolve<TType>(params object[] arguments)
        {
            return (TType)this.Resolve(typeof(TType), arguments);
        }

        void IContainerInstanceStore.OnRegistrationContextDisposing(IRegistrationContext registrationContext)
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

        void IContainerInstanceStore.Add(Type type, ContainerInstance instance, IRegistrationContext registrationContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (this.currentRegistrationContext == null || this.currentRegistrationContext != registrationContext)
            {
                throw new NotSupportedException(FrameworkResources.ErrMsg_ParentRegistrationContextIsDisposed);
            }

            this.registeredObjects.Add(type, instance);
        }

        ContainerInstance IContainerInstanceStore.Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ContainerInstance instance;
            return this.registeredObjects.TryGetValue(type, out instance) ? instance : null;
        }
    }
}