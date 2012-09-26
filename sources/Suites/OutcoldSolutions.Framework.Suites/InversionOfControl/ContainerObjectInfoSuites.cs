// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    public class ContainerObjectInfoSuites
    {
        private Type registeredType;
        private Mock<IDependencyResolverContainerEx> container;
        private Mock<IRegistrationContext> registrationContext;

        [SetUp]
        public void TestInitialization()
        {
            this.registeredType = typeof(Encoding);
            this.container = new Mock<IDependencyResolverContainerEx>();
            this.registrationContext = new Mock<IRegistrationContext>();
        }

        [Test]
        public void Ctor_ClassShouldSelfRegister()
        {
            // Arrange + Act
            var objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);

            // Assert
            this.container.Verify(c => c.Add(this.registeredType, objectInfo, this.registrationContext.Object), Times.Once());
        }

        [Test]
        public void For_PassNullInstance_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);

            // Act
            TestDelegate act = () => objectInfo.And(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void For_PassNewType_ShouldPassItToContainer()
        {
            // Arrange
            var objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);
            var type = typeof(UTF8Encoding);

            // Act
            objectInfo.And(type);

            // Assert
            this.container.Verify(c => c.Add(type, objectInfo, this.registrationContext.Object), Times.Once());
        }
    }
}