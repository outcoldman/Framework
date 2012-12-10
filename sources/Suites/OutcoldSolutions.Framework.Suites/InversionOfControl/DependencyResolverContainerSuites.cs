// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;

    using NUnit.Framework;

    public class DependencyResolverContainerSuites
    {
        [Test]
        public void Register_AsInstance_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.Registration())
            {
                registrationContext.Register(typeof(IServiceStub1))
                        .AsSingleton(new ServiceStub());
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub1)));
            Assert.IsFalse(container.IsRegistered(typeof(ServiceStub)));
        }

        [Test]
        public void Register_AsSingleton_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.Registration())
            {
                registrationContext.Register(typeof(IServiceStub1))
                    .And(typeof(IServiceStub2))
                    .AsSingleton(typeof(ServiceStub));
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub1)));
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub2)));
            Assert.IsFalse(container.IsRegistered(typeof(ServiceStub)));
        }

        [Test]
        public void Register_ForTwoInterfaces_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.Registration())
            {
                registrationContext.Register(typeof(IServiceStub1))
                        .And(typeof(IServiceStub2))
                        .AsSingleton(new ServiceStub());
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub1)));
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub2)));
            Assert.IsFalse(container.IsRegistered(typeof(ServiceStub)));
        }

        [Test]
        public void Registration_TryToGetSecondContext_ThrowsException()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            container.Registration();

            TestDelegate act = () => container.Registration();

            // Assert
            Assert.Throws<NotSupportedException>(act);
        }

        [Test]
        public void Registration_TryToGetSecondContextAfterFirstDisposed_ShouldProvideContext()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            var registrationContext1 = container.Registration();
            registrationContext1.Dispose();

            var registrationContext2 = container.Registration();

            // Assert
            Assert.IsNotNull(registrationContext2);
        }

        [Test]
        public void Register_UseTwoRegistrationContextes_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.Registration())
            {
                registrationContext.Register(typeof(IServiceStub1))
                        .AsSingleton(typeof(ServiceStub));
            }

            using (var registrationContext = container.Registration())
            {
                registrationContext.Register(typeof(IServiceStub2))
                        .AsSingleton(typeof(ServiceStub));
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub1)));
            Assert.IsTrue(container.IsRegistered(typeof(IServiceStub2)));
            Assert.IsFalse(container.IsRegistered(typeof(ServiceStub)));
        }

        [Test]
        public void Dispose_SelfRegistered_ShouldNotDisposeItself()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            container.Dispose();
        }
    }
}
