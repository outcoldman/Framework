// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    internal class ContainerObjectInfo : IContainerObjectInfo
    {
        private readonly Container container;

        public ContainerObjectInfo(Container container)
        {
            this.container = container;
        }

        public bool IsSingleton { get; private set; }

        public object Instance { get; private set; }

        public IContainerObjectInfo For(Type type)
        {
            this.container.Add(type, this);
            return this;
        }

        public void AsSingleton()
        {
            this.IsSingleton = true;
        }

        public void AsSingleton(object instance)
        {
            this.Instance = instance;
        }
    }
}