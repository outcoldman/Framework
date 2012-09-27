// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    public class ContainerInstanceSuites
    {
        private Type registeredType;
        private Mock<IContainerInstanceStore> store;
        private Mock<IDependencyResolverContainer> container;
        private Mock<IRegistrationContext> registrationContext;

        [SetUp]
        public void TestInitialization()
        {
            this.registeredType = typeof(Encoding);
            this.store = new Mock<IContainerInstanceStore>();
            this.registrationContext = new Mock<IRegistrationContext>();
            this.container = new Mock<IDependencyResolverContainer>();
        }

        [Test]
        public void Ctor_ClassShouldSelfRegister()
        {
            // Arrange + Act
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Assert
            this.store.Verify(c => c.Add(this.registeredType, objectInfo, this.registrationContext.Object), Times.Once());
        }

        [Test]
        public void For_PassNullInstance_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            TestDelegate act = () => objectInfo.And(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void For_PassNewType_ShouldPassItToContainer()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var type = typeof(UTF8Encoding);

            // Act
            objectInfo.And(type);

            // Assert
            this.store.Verify(c => c.Add(type, objectInfo, this.registrationContext.Object), Times.Once());
        }
    }
}