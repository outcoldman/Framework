// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class Container
    {
        private readonly IDictionary<Type, IContainerObjectInfo> registeredObjects = new Dictionary<Type, IContainerObjectInfo>();
        private readonly object locker = new object();
        private IRegistrationContext currentRegistrationContext;

        public bool TryLockForRegistrationContext(out IRegistrationContext registrationContext)
        {
            if (Monitor.TryEnter(this.locker) && this.currentRegistrationContext == null)
            {
                registrationContext = this.currentRegistrationContext = new RegistrationContext(this);
                return true;
            }

            registrationContext = null;
            return false;
        }

        public void UnlockRegistrationContext()
        {
            this.currentRegistrationContext = null;
            Monitor.Exit(this.locker);
        }

        public void Add(Type type, ContainerObjectInfo objectInfo)
        {
            if (this.currentRegistrationContext == null)
            {
                throw new NotSupportedException("Registration context is not active.");
            }

            this.registeredObjects.Add(type, objectInfo);
        }

        public bool IsRegistered(Type type)
        {
            return this.registeredObjects.ContainsKey(type);
        }
    }
}
