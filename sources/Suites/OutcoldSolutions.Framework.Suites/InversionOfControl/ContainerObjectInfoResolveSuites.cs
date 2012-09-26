// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    public class ContainerObjectInfoResolveSuites
    {
        private Type registeredType;
        private Mock<IDependencyResolverContainerEx> container;
        private Mock<IRegistrationContext> registrationContext;
        private ContainerObjectInfo objectInfo;

        [SetUp]
        public void TestInitialization()
        {
            this.registeredType = typeof(Encoding);
            this.container = new Mock<IDependencyResolverContainerEx>();
            this.registrationContext = new Mock<IRegistrationContext>();
            this.objectInfo = new ContainerObjectInfo(this.registeredType, this.container.Object, this.registrationContext.Object);
        }

        [Test]
        public void Resolve_SetFactory_ShouldBeResolvedWithFactory()
        {
            // Arrange
            var instance = new UTF8Encoding();
            this.objectInfo.As((arguments) => instance);

            // Act
            object result = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(instance, result);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldBeResolvedWithFactory()
        {
            // Arrange
            var instance = new UTF8Encoding();
            this.objectInfo.AsSingleton((arguments) => instance);

            // Act
            object result = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(instance, result);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldReturnSingleton()
        {
            // Arrange
            this.objectInfo.AsSingleton((arguments) => new UTF8Encoding());

            // Act
            object result1 = this.objectInfo.Resolve();
            object result2 = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetFactory_ShouldPassTheSameArrayOfParameters()
        {
            // Arrange
            var arguments = new object[10];
            object[] factoryArguments = null;
            this.objectInfo.As((a) =>
                { 
                    factoryArguments = a;
                    return new UTF8Encoding();
                });

            // Act
            this.objectInfo.Resolve(arguments);

            // Assert
            Assert.AreSame(arguments, factoryArguments);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldPassTheSameArrayOfParameters()
        {
            // Arrange
            var arguments = new object[10];
            object[] factoryArguments = null;
            this.objectInfo.AsSingleton((a) =>
            {
                factoryArguments = a;
                return new UTF8Encoding();
            });

            // Act
            this.objectInfo.Resolve(arguments);

            // Assert
            Assert.AreSame(arguments, factoryArguments);
        }

        [Test]
        public void Resolve_SetInstance_ShouldBeResolvedWithTheSameInstance()
        {
            // Arrange
            var instance = new UTF8Encoding();
            this.objectInfo.AsSingleton(instance);

            // Act
            object result = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(instance, result);
        }

        [Test]
        public void Resolve_SetInstance_ShouldReturnSingleton()
        {
            // Arrange
            this.objectInfo.AsSingleton(new UTF8Encoding());

            // Act
            object result1 = this.objectInfo.Resolve();
            object result2 = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetSingletonType_ShouldReturnObjectWithTheSameType()
        {
            // Arrange
            var type = typeof(ServiceStub);
            this.objectInfo.AsSingleton(type);

            // Act
            object result = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(type, result.GetType());
        }

        [Test]
        public void Resolve_SetSingletonType_ShouldReturnSingleton()
        {
            // Arrange
            this.objectInfo.AsSingleton(typeof(ServiceStub));

            // Act
            object result1 = this.objectInfo.Resolve();
            object result2 = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetType_ShouldReturnObjectWithTheSameType()
        {
            // Arrange
            var type = typeof(ServiceStub);
            this.objectInfo.As(type);

            // Act
            object result = this.objectInfo.Resolve();

            // Assert
            Assert.AreSame(type, result.GetType());
        }

        [Test]
        public void Resolve_SetType_ShouldReturnAlwaysNewInstance()
        {
            // Arrange
            this.objectInfo.As(typeof(ServiceStub));

            // Act
            object result1 = this.objectInfo.Resolve();
            object result2 = this.objectInfo.Resolve();

            // Assert
            Assert.AreNotSame(result1, result2);
        }

        [Test]
        public void Resolve_WithArguments_ShouldUseArguments()
        {
            // Arrange
            string a = "Test";
            int b = 10;
            this.objectInfo.As(typeof(ServiceWithConstructorStub));

            // Act
            var result = (ServiceWithConstructorStub)this.objectInfo.Resolve(new object[] { a, b });

            // Assert
            Assert.AreEqual(a, result.A);
            Assert.AreEqual(b, result.B);
        }

        [Test]
        public void Resolve_TypeWithMoreThanOneConstructors_ShouldUseCtorWithAttribute()
        {
            // Arrange
            string a = "Test";
            this.objectInfo.As(typeof(ServiceWithConstructorsStub));

            // Act
            var result = (ServiceWithConstructorsStub)this.objectInfo.Resolve(new object[] { a, 10 });

            // Assert
            Assert.AreEqual(a, result.A);
            Assert.IsNull(result.B);
        }
    }
}