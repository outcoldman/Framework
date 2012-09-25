// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyResolverContainerSuites
    {
        [TestMethod]
        public void Register_AsInstance_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.GetRegistrationContext())
            {
                registrationContext.Register(typeof(Service))
                    .For(typeof(IService1))
                    .For(typeof(IService2))
                        .AsSingleton(new Service());
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IService1)));
            Assert.IsTrue(container.IsRegistered(typeof(IService2)));
            Assert.IsTrue(container.IsRegistered(typeof(Service)));
        }

        [TestMethod]
        public void Register_AsSingleton_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.GetRegistrationContext())
            {
                registrationContext.Register(typeof(Service))
                    .For(typeof(IService1))
                    .For(typeof(IService2))
                        .AsSingleton();
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IService1)));
            Assert.IsTrue(container.IsRegistered(typeof(IService2)));
            Assert.IsTrue(container.IsRegistered(typeof(Service)));
        }

        [TestMethod]
        public void Register_ForTwoInterfaces_ShouldBeRegistered()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            using (var registrationContext = container.GetRegistrationContext())
            {
                registrationContext.Register(typeof(Service))
                    .For(typeof(IService1))
                    .For(typeof(IService2))
                        .AsSingleton();
            }

            // Assert
            Assert.IsTrue(container.IsRegistered(typeof(IService1)));
            Assert.IsTrue(container.IsRegistered(typeof(IService2)));
            Assert.IsTrue(container.IsRegistered(typeof(Service)));
        }

        [TestMethod]
        public void GetRegistrationContext_TryToGetSecondContext_ThrowsException()
        {
            // Arrange
            var container = new DependencyResolverContainer();

            // Act
            var registrationContext1 = container.GetRegistrationContext();

            Action act = () =>
                {
                    var registrationContext2 = container.GetRegistrationContext();
                };

            // Assert
            AssertEx.Throws<NotSupportedException>(act);
        }

        [TestMethod]
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
        
        public interface IService1
        {
        }

        public interface IService2
        {
        }

        public class Service : IService1, IService2
        {
        }
    }
}
