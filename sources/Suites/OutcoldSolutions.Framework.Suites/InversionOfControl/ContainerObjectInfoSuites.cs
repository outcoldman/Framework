// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ContainerObjectInfoSuites
    {
        private Type registeredType;
        private Mock<IDependencyResolverContainerEx> container;
        private Mock<IRegistrationContext> registrationContext;

        [TestInitialize]
        public void TestInitialization()
        {
            this.registeredType = typeof(Encoding);
            this.container = new Mock<IDependencyResolverContainerEx>();
            this.registrationContext = new Mock<IRegistrationContext>();
        }

        [TestMethod]
        public void Ctor_ClassShouldSelfRegister()
        {
            // Arrange + Act
            var objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);

            // Assert
            this.container.Verify(c => c.Add(this.registeredType, objectInfo, this.registrationContext.Object), Times.Once());
        }

        [TestMethod]
        public void For_PassNullInstance_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);

            // Act
            Action act = () => objectInfo.And(null);

            // Assert
            AssertEx.Throws<ArgumentNullException>(act);
        }

        [TestMethod]
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