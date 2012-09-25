// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    internal class RegistrationContext : IRegistrationContext
    {
        private readonly Container container;

        public RegistrationContext(Container container)
        {
            this.container = container;
        }

        public void Dispose()
        {
            this.container.UnlockRegistrationContext();
        }

        public IContainerObjectInfo Register(Type typeImplementation)
        {
            var info = new ContainerObjectInfo(this.container);
            this.container.Add(typeImplementation, info);
            return info;
        }
    }
}