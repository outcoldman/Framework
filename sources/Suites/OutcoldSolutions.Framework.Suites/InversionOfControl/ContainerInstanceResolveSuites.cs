// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    public class ContainerInstanceResolveSuites
    {
        private Type registeredType;
        private Mock<IContainerInstanceStore> store;
        private Mock<IDependencyResolverContainer> container;
        private Mock<IRegistrationContext> registrationContext;
        private ContainerInstance instance;

        [SetUp]
        public void TestInitialization()
        {
            this.registeredType = typeof(IServiceStub1);
            this.store = new Mock<IContainerInstanceStore>();
            this.container = new Mock<IDependencyResolverContainer>();
            this.registrationContext = new Mock<IRegistrationContext>();
            this.instance = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
        }

        [Test]
        public void Resolve_SetFactory_ShouldBeResolvedWithFactory()
        {
            // Arrange
            var obj = new ServiceStub();
            this.instance.As(arguments => obj);

            // Act
            object result = this.instance.Resolve();

            // Assert
            Assert.AreSame(obj, result);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldBeResolvedWithFactory()
        {
            // Arrange
            var obj = new ServiceStub();
            this.instance.AsSingleton(arguments => obj);

            // Act
            object result = this.instance.Resolve();

            // Assert
            Assert.AreSame(obj, result);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldReturnSingleton()
        {
            // Arrange
            this.instance.AsSingleton(arguments => new ServiceStub());

            // Act
            object result1 = this.instance.Resolve();
            object result2 = this.instance.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetFactory_ShouldPassTheSameArrayOfParameters()
        {
            // Arrange
            var arguments = new object[10];
            object[] factoryArguments = null;
            this.instance.As(a =>
                { 
                    factoryArguments = a;
                    return new ServiceStub();
                });

            // Act
            this.instance.Resolve(arguments);

            // Assert
            Assert.AreSame(arguments, factoryArguments);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldPassTheSameArrayOfParameters()
        {
            // Arrange
            var arguments = new object[10];
            object[] factoryArguments = null;
            this.instance.AsSingleton(a =>
            {
                factoryArguments = a;
                return new ServiceStub();
            });

            // Act
            this.instance.Resolve(arguments);

            // Assert
            Assert.AreSame(arguments, factoryArguments);
        }

        [Test]
        public void Resolve_SetInstance_ShouldBeResolvedWithTheSameInstance()
        {
            // Arrange
            var obj = new ServiceStub();
            this.instance.AsSingleton(obj);

            // Act
            object result = this.instance.Resolve();

            // Assert
            Assert.AreSame(obj, result);
        }

        [Test]
        public void Resolve_SetInstance_ShouldReturnSingleton()
        {
            // Arrange
            this.instance.AsSingleton(new ServiceStub());

            // Act
            object result1 = this.instance.Resolve();
            object result2 = this.instance.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetSingletonType_ShouldReturnObjectWithTheSameType()
        {
            // Arrange
            var type = typeof(ServiceStub);
            this.instance.AsSingleton(type);

            // Act
            object result = this.instance.Resolve();

            // Assert
            Assert.AreSame(type, result.GetType());
        }

        [Test]
        public void Resolve_SetSingletonType_ShouldReturnSingleton()
        {
            // Arrange
            this.instance.AsSingleton(typeof(ServiceStub));

            // Act
            object result1 = this.instance.Resolve();
            object result2 = this.instance.Resolve();

            // Assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void Resolve_SetType_ShouldReturnObjectWithTheSameType()
        {
            // Arrange
            var type = typeof(ServiceStub);
            this.instance.As(type);

            // Act
            object result = this.instance.Resolve();

            // Assert
            Assert.AreSame(type, result.GetType());
        }

        [Test]
        public void Resolve_SetType_ShouldReturnAlwaysNewInstance()
        {
            // Arrange
            this.instance.As(typeof(ServiceStub));

            // Act
            object result1 = this.instance.Resolve();
            object result2 = this.instance.Resolve();

            // Assert
            Assert.AreNotSame(result1, result2);
        }

        [Test]
        public void Resolve_WithArguments_ShouldUseArguments()
        {
            // Arrange
            const string A = "Test";
            const int B = 10;
            this.instance.As(typeof(ServiceWithConstructorStub));

            // Act
            var result = (ServiceWithConstructorStub)this.instance.Resolve(new object[] { A, B });

            // Assert
            Assert.AreEqual(A, result.A);
            Assert.AreEqual(B, result.B);
        }

        [Test]
        public void Resolve_TypeWithMoreThanOneConstructors_ShouldUseCtorWithAttribute()
        {
            // Arrange
            const string A = "Test";
            this.instance.As(typeof(ServiceWithConstructorsStub));

            // Act
            var result = (ServiceWithConstructorsStub)this.instance.Resolve(new object[] { A, 10 });

            // Assert
            Assert.AreEqual(A, result.A);
            Assert.IsNull(result.B);
        }
    }
}