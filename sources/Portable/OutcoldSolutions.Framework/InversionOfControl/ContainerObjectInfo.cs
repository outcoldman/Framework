// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class ContainerObjectInfo : IContainerObjectInfo
    {
        private const string CompiledExpressionInputParameterName = "a";

        private readonly IDependencyResolverContainerEx container;
        private readonly List<Type> registeredTypes = new List<Type>();
        private readonly object typeLocker = new object();

        private bool isResolving;
        private IRegistrationContext registrationContext;
        private Type implementation;
        private Func<object[], object> factory;
        private object instance;

        private List<InjectInfo> requiredInjections;

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
                this.IsSingleton = true;
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
                        Debug.Assert(this.IsSingleton, "this.IsSingleton");
                        return this.instance;
                    }

                    object result = null;

                    // If we don't have a factory and we don't initialized this.requiredInjections
                    // we need to find a type which can construct objects and compile function
                    // which can create new instances of this type.
                    if (this.factory == null && this.requiredInjections == null)
                    {
                        this.Compile();
                    }

                    if (this.factory == null)
                    {
                        throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_CannotFindConstructorForType, this.implementation));
                    }

                    object[] ctorArguments;

                    if (this.requiredInjections != null)
                    {
                        ctorArguments = this.requiredInjections.Select(
                            p =>
                                {
                                    object value = null;
                                    if (arguments != null)
                                    {
                                        value = arguments.FirstOrDefault(p.Type.IsInstanceOfType);
                                    }

                                    if (value == null)
                                    {
                                        if (p.ContainerObjectInfo == null)
                                        {
                                            ContainerObjectInfo objectInfo;
                                            value = this.container.ResolveAndGet(p.Type, arguments, out objectInfo);
                                            p.ContainerObjectInfo = objectInfo;
                                        }
                                        else
                                        {
                                            value = p.ContainerObjectInfo.Resolve(arguments);
                                        }
                                    }

                                    return value;
                                }).ToArray();
                    }
                    else
                    {
                        ctorArguments = arguments;
                    }

                    result = this.factory(ctorArguments);
                    
                    if (this.IsSingleton)
                    {
                        // In case if this is singleton we will remember value
                        // and will clear all unnecessary objects.
                        this.instance = result;
                        this.factory = null;
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

                // If we have more than 1 constructor we will not compile function
                // and will throw exception because this.factory will be null.
                if (constructorInfos.Length == 1)
                {
                    var constructorInfo = constructorInfos[0];

                    var parameterTypes = constructorInfo.GetParameters().Select(info => info.ParameterType).ToList();
                    this.requiredInjections = parameterTypes.Select(t => new InjectInfo(t, this.container.Get(t))).ToList();

                    // Compile a new lambda expression:
                    // ((object[]) a) => 
                    // {
                    //    return ctor((TType0)a[0], (TType1)a[1], ... , (TTypeN)a[N]);
                    // }
                    ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), CompiledExpressionInputParameterName);
                    var expressions = parameterTypes.Select((t, i) => Expression.Convert(Expression.ArrayIndex(parameterExpression, Expression.Constant(i)), t)).Cast<Expression>().ToList();

                    NewExpression newExp = Expression.New(constructorInfo, expressions);
                    LambdaExpression lambda = Expression.Lambda(newExp, parameterExpression);
                    this.factory = (Func<object[], object>)lambda.Compile();
                }
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

        private class InjectInfo
        {
            public InjectInfo(Type type, ContainerObjectInfo containerObjectInfo)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }

                this.Type = type;
                this.ContainerObjectInfo = containerObjectInfo;
            }

            public Type Type { get; private set; }

            public ContainerObjectInfo ContainerObjectInfo { get; set; }
        }
    }
}