// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Globalization;
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
            this.registeredType = typeof(IServiceStub1);
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
        public void And_PassNullInstance_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            TestDelegate act = () => objectInfo.And(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void And_PassNewType_ShouldPassItToContainer()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var type = typeof(IServiceStub2);

            // Act
            objectInfo.And(type);

            // Assert
            this.store.Verify(c => c.Add(type, objectInfo, this.registrationContext.Object), Times.Once());
        }

        [Test]
        public void GenericAnd_PassNewType_ShouldPassItToContainer()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            objectInfo.And<IServiceStub2>();

            // Assert
            this.store.Verify(c => c.Add(typeof(IServiceStub2), objectInfo, this.registrationContext.Object), Times.Once());
        }

        [Test]
        public void As_SetInterfaceAsTypeImplementation_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            TestDelegate testDelegate = () => objectInfo.As(typeof(IServiceStubParent1));

            // Assert
            Assert.Throws<ArgumentException>(
                testDelegate,
                string.Format(
                    CultureInfo.CurrentCulture,
                    InversionOfControlResources.ErrMsg_CannotSetInterfaceAsImplementation,
                    typeof(IServiceStubParent1)));
        }

        [Test]
        public void As_SetTypeImplementationWithWrongType_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            TestDelegate testDelegate = () => objectInfo.As(typeof(UTF8Encoding));

            // Assert
            Assert.Throws<ArgumentException>(
                testDelegate,
                string.Format(
                    CultureInfo.CurrentCulture,
                    InversionOfControlResources.ErrMsg_CannotSetTypeAsImplementation,
                    typeof(UTF8Encoding),
                    this.registeredType));
        }

        [Test]
        public void As_SetTypeImplementationWithWrongTypeForOneOfTwoServices_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.And<IServiceProvider>();

            // Act
            TestDelegate testDelegate = () => objectInfo.As(typeof(ServiceStub));

            // Assert
            Assert.Throws<ArgumentException>(
                testDelegate,
                string.Format(
                    CultureInfo.CurrentCulture,
                    InversionOfControlResources.ErrMsg_CannotSetTypeAsImplementation,
                    typeof(ServiceStub),
                    typeof(IServiceProvider)));
        }

        [Test]
        public void Resolve_SetTypeImplementation_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As(typeof(ServiceStub));

            // Act
            var serviceStub = objectInfo.Resolve();

            // Assert
            Assert.IsInstanceOf(typeof(ServiceStub), serviceStub);
        }

        [Test]
        public void Resolve_SetTypeImplementation_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As(typeof(ServiceStub));

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreNotSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetTypeImplementationWithGenericMethod_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As<ServiceStub>();

            // Act
            var serviceStub = objectInfo.Resolve();

            // Assert
            Assert.IsInstanceOf(typeof(ServiceStub), serviceStub);
        }

        [Test]
        public void Resolve_SetTypeImplementationWithGenericMethod_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As<ServiceStub>();

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreNotSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetFactory_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var serviceStub = new ServiceStub();
            objectInfo.As(() => serviceStub);

            // Act
            var result = objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub, result);
        }

        [Test]
        public void Resolve_SetFactory_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As(() => new ServiceStub());

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreNotSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetFactoryWithArguments_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var serviceStub = new ServiceStub();
            objectInfo.As((arguments) => serviceStub);

            // Act
            var result = objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub, result);
        }

        [Test]
        public void Resolve_SetFactoryWithArguments_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.As((arguments) => new ServiceStub());

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreNotSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetTypeImplementationAsSingleton_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton(typeof(ServiceStub));

            // Act
            var serviceStub = objectInfo.Resolve();

            // Assert
            Assert.IsInstanceOf(typeof(ServiceStub), serviceStub);
        }

        [Test]
        public void Resolve_SetTypeImplementationAsSingleton_ShouldCreateTheSameInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton(typeof(ServiceStub));

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetTypeImplementationAsSingletonWithGenericMethod_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton<ServiceStub>();

            // Act
            var serviceStub = objectInfo.Resolve();

            // Assert
            Assert.IsInstanceOf(typeof(ServiceStub), serviceStub);
        }

        [Test]
        public void Resolve_SetTypeImplementationAsSingletonWithGenericMethod_ShouldCreateTheSameInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton<ServiceStub>();

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var serviceStub = new ServiceStub();
            objectInfo.AsSingleton(() => serviceStub);

            // Act
            var result = objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub, result);
        }

        [Test]
        public void Resolve_SetFactoryAsSingleton_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton(() => new ServiceStub());

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_SetFactoryWithArgumentsAsSingleton_ShouldCreateNewInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var serviceStub = new ServiceStub();
            objectInfo.AsSingleton((arguments) => serviceStub);

            // Act
            var result = objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub, result);
        }

        [Test]
        public void Resolve_SetFactoryWithArgumentsAsSingleton_ShouldCreateNewInstanceEachTime()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            objectInfo.AsSingleton((arguments) => new ServiceStub());

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_TypeDoesntHaveSpecifiedConstructor_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(typeof(ServiceWithoutInjectAttributeStub), this.store.Object, this.registrationContext.Object, this.container.Object);

            // Act
            TestDelegate action = () => objectInfo.Resolve();

            // Assert
            Assert.Throws<NotSupportedException>(action, string.Format(CultureInfo.CurrentCulture, InversionOfControlResources.ErrMsg_CannotFindConstructorForType, typeof(ServiceWithoutInjectAttributeStub)));
        }

        [Test]
        public void Resolve_SetInstanceAsSingleton_ShouldReturnTheSameInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.store.Object, this.registrationContext.Object, this.container.Object);
            var instance = new ServiceStub();
            objectInfo.AsSingleton(instance);

            // Act
            var serviceStub1 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(instance, serviceStub1);
        }
    }
}