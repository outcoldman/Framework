// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class ContainerObjectInfo : IContainerObjectInfo
    {
        private readonly IDependencyResolverContainerEx container;
        private readonly List<Type> registeredTypes = new List<Type>();
        private readonly object typeLocker = new object();

        private bool isResolving;
        private IRegistrationContext registrationContext;
        private Type implementation;
        private Func<object[], object> factory;
        private object instance;

        private ConstructorInfo constructorInfo;
        private List<Type> requiredInjections;

        public ContainerObjectInfo(Type type, IDependencyResolverContainerEx container, IRegistrationContext registrationContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (registrationContext == null)
            {
                throw new ArgumentNullException("registrationContext");
            }

            this.container = container;
            this.registrationContext = registrationContext;

            this.And(type);
        }

        public bool IsSingleton { get; private set; }

        public IContainerObjectInfo And(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            lock (this.typeLocker)
            {
                this.CheckRegistrationContext();

                this.container.Add(type, this, this.registrationContext);
                this.registeredTypes.Add(type);
                return this;
            }
        }

        public void As(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.implementation = typeImplementation;
            }
        }

        public void As(Func<object[], object> factoryFunction)
        {
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.factory = factoryFunction;
            }
        }

        public void AsSingleton(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.implementation = typeImplementation;
                this.IsSingleton = true;
            }
        }

        public void AsSingleton(object instanceObject)
        {
            if (instanceObject == null)
            {
                throw new ArgumentNullException("instanceObject");
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.instance = instanceObject;
            }
        }

        public void AsSingleton(Func<object[], object> factoryFunction)
        {
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.factory = factoryFunction;
                this.IsSingleton = true;
            }
        }

        public object Resolve(object[] arguments = null)
        {
            lock (this.typeLocker)
            {
                if (this.isResolving)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_CircularResolving, this.implementation));
                }

                try
                {
                    this.isResolving = true;

                    this.registrationContext = null;

                    if (this.instance != null)
                    {
                        return this.instance;
                    }

                    object result = null;

                    if (this.factory != null)
                    {
                        result = this.factory(arguments);
                    }
                    else
                    {
                        if (this.constructorInfo == null)
                        {
                            this.Compile();
                        }

                        var ctorParameters = this.requiredInjections.Select(
                            p =>
                            {
                                object argumentValue = null;
                                if (arguments != null)
                                {
                                    argumentValue = arguments.FirstOrDefault(p.IsInstanceOfType);
                                }

                                return argumentValue ?? this.container.Resolve(p);
                            });

                        NewExpression newExp = Expression.New(
                            this.constructorInfo, ctorParameters.Select(x => (Expression)Expression.Constant(x)));
                        LambdaExpression lambda = Expression.Lambda(typeof(Func<object>), newExp);
                        var compiled = (Func<object>)lambda.Compile();

                        result = compiled();
                    }

                    if (this.IsSingleton)
                    {
                        this.instance = result;
                        this.factory = null;
                        this.constructorInfo = null;
                        this.requiredInjections = null;
                    }

                    return result;
                }
                finally
                {
                    this.isResolving = false;
                }
            }
        }

        private void Compile()
        {
            if (this.factory == null && this.instance == null)
            {
                if (this.implementation == null)
                {
                    this.implementation = this.registeredTypes.First();
                }

                ConstructorInfo[] constructorInfos = this.implementation.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (constructorInfos.Length > 1)
                {
                    constructorInfos = constructorInfos.Where(info =>
                    {
                        object[] customAttributes = info.GetCustomAttributes(typeof(InjectAttribute), false);
                        return customAttributes != null && customAttributes.Length > 0;
                    }).ToArray();
                }

                if (constructorInfos.Length != 1)
                {
                    throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_CannotFindConstructorForType, this.implementation));
                }

                this.constructorInfo = constructorInfos[0];

                this.requiredInjections = this.constructorInfo.GetParameters().Select(info => info.ParameterType).ToList();
            }
        }

        private void CheckState()
        {
            this.CheckBehavior();
            this.CheckRegistrationContext();
        }

        private void CheckBehavior()
        {
            if (this.factory != null || this.implementation != null || this.instance != null)
            {
                throw new NotSupportedException(FrameworkResources.ErrMsg_CannotSetMoreThanOneBehavior);
            }
        }

        private void CheckRegistrationContext()
        {
            if (this.registrationContext == null || this.registrationContext.IsDisposed)
            {
                throw new ObjectDisposedException(typeof(IRegistrationContext).Name);
            }
        }
    }
}