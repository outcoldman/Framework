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

        private Dictionary<Type, ContainerObjectInfo> requiredInjections;
        private Delegate compiledConstructor;

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
                        if (this.compiledConstructor == null)
                        {
                            this.InitializeConstructor();
                        }

                        var parameterValues = this.requiredInjections.ToDictionary(
                            p => p.Key,
                            p =>
                            {
                                object value = null;
                                if (arguments != null)
                                {
                                    value = arguments.FirstOrDefault(p.Key.IsInstanceOfType);
                                }

                                if (value == null)
                                {
                                    if (p.Value == null)
                                    {
                                        value = this.container.Resolve(p.Key, arguments);
                                    }
                                    else
                                    {
                                        value = p.Value.Resolve(arguments);
                                    }
                                }

                                return value;
                            });
                        
                        if (parameterValues.Values.Count == 0)
                        {
                            result = this.compiledConstructor.DynamicInvoke();

                            if (this.requiredInjections.Count == 0)
                            {
                                var func = (Func<object>)this.compiledConstructor;
                                this.factory = (a) => func();
                            }
                        }
                        else
                        {
                            result = this.compiledConstructor.DynamicInvoke(parameterValues.Values.ToArray());
                        }
                    }

                    if (this.IsSingleton)
                    {
                        this.instance = result;
                        this.factory = null;
                        this.compiledConstructor = null;
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

        private void InitializeConstructor()
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

                var constructorInfo = constructorInfos[0];

                this.requiredInjections = constructorInfo.GetParameters().Select(info => info.ParameterType).ToDictionary(t => t, t => this.container.Get(t));

                var expressions = new List<ParameterExpression>();
                foreach (var injection in this.requiredInjections)
                {
                    expressions.Add(Expression.Parameter(injection.Key, "p" + expressions.Count));
                }

                NewExpression newExp = Expression.New(constructorInfo, expressions.Cast<Expression>());
                LambdaExpression lambda = Expression.Lambda(newExp, expressions.ToArray());
                this.compiledConstructor = lambda.Compile();
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