// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using System;
    using System.Globalization;
    using System.Text;

    using NUnit.Framework;

    public class ContainerInstanceSuites
    {
        private Type registeredType;
        private DependencyResolverContainer container;

        private IRegistrationContext registrationContext;

        [SetUp]
        public void TestInitialization()
        {
            this.registeredType = typeof(IServiceStub1);
            this.container = new DependencyResolverContainer();
            this.registrationContext = this.container.Registration();
        }

        [TearDown]
        public void TestUnInitialization()
        {
            this.registrationContext.Dispose();
        }

        [Test]
        public void Ctor_ShouldRegisterBaseTypeWithContainer()
        {
            // Arrange + Act
            var objectInfo = new ContainerInstance(this.registeredType, this.container);

            // Assert
            Assert.IsTrue(this.container.IsRegistered(this.registeredType));
        }

        [Test]
        public void And_PassNullInstance_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);

            // Act
            TestDelegate act = () => objectInfo.And(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void And_PassNewType_ShouldRegisterTypeWithContainer()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
            var type = typeof(IServiceStub2);

            // Act
            objectInfo.And(type);

            // Assert
            Assert.IsTrue(this.container.IsRegistered<IServiceStub2>());
        }

        [Test]
        public void GenericAnd_PassNewType_ShouldRegisterTypeWithContainer()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);

            // Act
            objectInfo.And<IServiceStub2>();

            // Assert
            Assert.IsTrue(this.container.IsRegistered<IServiceStub2>());
        }

        [Test]
        public void As_SetInterfaceAsTypeImplementation_ThrowsException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);

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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);

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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
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
            var objectInfo = new ContainerInstance(typeof(ServiceWithoutInjectAttributeStub), this.container);

            // Act
            TestDelegate action = () => objectInfo.Resolve();

            // Assert
            Assert.Throws<NotSupportedException>(action, string.Format(CultureInfo.CurrentCulture, InversionOfControlResources.ErrMsg_CannotFindConstructorForType, typeof(ServiceWithoutInjectAttributeStub)));
        }

        [Test]
        public void Resolve_SetInstanceAsSingleton_ShouldReturnTheSameInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
            var instance = new ServiceStub();
            objectInfo.AsSingleton(instance);

            // Act
            var serviceStub1 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(instance, serviceStub1);
        }

        [Test]
        public void Resolve_AsSingletonWithoutImplementation_ShouldReturnTheSameInstance()
        {
            // Arrange
            var objectInfo = new ContainerInstance(typeof(ServiceStub), this.container);
            objectInfo.AsSingleton();

            // Act
            var serviceStub1 = objectInfo.Resolve();
            var serviceStub2 = objectInfo.Resolve();

            // Assert
            Assert.AreSame(serviceStub1, serviceStub2);
        }

        [Test]
        public void Resolve_AsSingletonWithoutImplementationForMoreThanOneType_ThrowException()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
            objectInfo.And(typeof(ServiceStub));

            // Act
            TestDelegate act = objectInfo.AsSingleton;

            // Assert
            Assert.Throws<NotSupportedException>(act, InversionOfControlResources.ErrMsg_CannotRegisterTypeAsSingleton);
        }

        [Test]
        public void Resolve_WithArguments_ShouldUseArguments()
        {
            // Arrange
            var objectInfo = new ContainerInstance(this.registeredType, this.container);
            const string A = "Test";
            const int B = 10;
            objectInfo.As(typeof(ServiceWithConstructorStub));

            // Act
            var result = (ServiceWithConstructorStub)objectInfo.Resolve(new object[] { A, B });

            // Assert
            Assert.AreEqual(A, result.A);
            Assert.AreEqual(B, result.B);
        }

        [Test]
        public void Resolve_ResolveTypeWithDependency_ShouldAskContainerToResolveArgumentsForCtor()
        {
            // Arrange
            var serviceStub = new ServiceStub();
            this.registrationContext.Register<IServiceStub1>().AsSingleton(serviceStub);

            var objectInfo = new ContainerInstance(typeof(ServiceWithDependencyStub), this.container);
            objectInfo.As(typeof(ServiceWithDependencyStub));

            // Act
            var result = (ServiceWithDependencyStub)objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub, result.Child);
        }

        [Test]
        public void Resolve_TypeWithMethodInjection_ShouldInjectMethod()
        {
            // Arrange
            var serviceStub1 = new ServiceStub();
            var serviceStub2 = new ServiceStub();
            this.registrationContext.Register<IServiceStub1>().AsSingleton(serviceStub1);
            this.registrationContext.Register<IServiceStub2>().AsSingleton(serviceStub2);

            var objectInfo = new ContainerInstance(typeof(ServiceWithMethodInjection), this.container);
            objectInfo.As(typeof(ServiceWithMethodInjection));

            // Act
            var result = (ServiceWithMethodInjection)objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub1, result.Child);
            Assert.AreEqual(serviceStub2, result.Child2);
        }

        [Test]
        public void Resolve_TypeWithMethodWithReturnInjection_ShouldInjectMethod()
        {
            // Arrange
            var serviceStub1 = new ServiceStub();
            this.registrationContext.Register<IServiceStub1>().AsSingleton(serviceStub1);

            var objectInfo = new ContainerInstance(typeof(ServiceWithMethodWithReturnInjection), this.container);
            objectInfo.As(typeof(ServiceWithMethodWithReturnInjection));

            // Act
            var result = (ServiceWithMethodWithReturnInjection)objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub1, result.Child);
        }

        [Test]
        public void Resolve_TypeWithPrivateMethodInjection_ShouldInjectMethod()
        {
            // Arrange
            var serviceStub1 = new ServiceStub();
            this.registrationContext.Register<IServiceStub1>().AsSingleton(serviceStub1);

            var objectInfo = new ContainerInstance(typeof(ServiceWithPrivateMethodInjection), this.container);
            objectInfo.As(typeof(ServiceWithPrivateMethodInjection));

            // Act
            var result = (ServiceWithPrivateMethodInjection)objectInfo.Resolve();

            // Assert
            Assert.AreEqual(serviceStub1, result.Child);
        }
    }
}