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
            using (var registrationContext = container.GetRegistrationContext())
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
            using (var registrationContext = container.GetRegistrationContext())
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
            using (var registrationContext = container.GetRegistrationContext())
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
        public void GetRegistrationContext_TryToGetSecondContext_ThrowsException()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            var registrationContext1 = container.GetRegistrationContext();

            TestDelegate act = () =>
                {
                    var registrationContext2 = container.GetRegistrationContext();
                };

            // Assert
            Assert.Throws<NotSupportedException>(act);
        }

        [Test]
        public void GetRegistrationContext_TryToGetSecondContextAfterFirstDisposed_ShouldProvideContext()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            var registrationContext1 = container.GetRegistrationContext();
            registrationContext1.Dispose();

            var registrationContext2 = container.GetRegistrationContext();

            // Assert
            Assert.IsNotNull(registrationContext2);
        }

        [Test]
        public void Register_UseTwoRegistrationContextes_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.GetRegistrationContext())
            {
                registrationContext.Register(typeof(IServiceStub1))
                        .AsSingleton(typeof(ServiceStub));
            }

            using (var registrationContext = container.GetRegistrationContext())
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
        public void Resolve_CircularType_ShouldThrowException()
        {
            // Arrange
            var container = new DependencyResolverContainer();
            using (var registrationContext = container.GetRegistrationContext())
            {
                registrationContext.Register(typeof(ServiceCircularStub));
            }

            // Act
            TestDelegate act = () => container.Resolve(typeof(ServiceCircularStub));

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }
    }
}
