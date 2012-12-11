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
    using System.Threading;

    /// <summary>
    /// Internal implementation for <see cref="IContainerInstance"/>.
    /// </summary>
    internal class ContainerInstance : IContainerInstance, IDisposable
    {
        private const string CompiledExpressionInputParameterName = "a";

        private readonly DependencyResolverContainer container;
        private readonly List<Type> registeredTypes = new List<Type>();
        private readonly object typeLocker = new object();
        
        private Type implementation;
        private bool isSingleton;
        private Func<object[], object> factory;
        private object instance;

        private List<InjectInfo> requiredInjections;

        private bool isDisposed;

        public ContainerInstance(
            Type type, 
            DependencyResolverContainer container)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.container = container;

            this.And(type);
        }

        ~ContainerInstance()
        {
            this.Dispose(disposing: false);
        }

        public IContainerInstance And(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            lock (this.typeLocker)
            {
                this.container.Add(type, this);
                this.registeredTypes.Add(type);
                return this;
            }
        }

        public IContainerInstance And<TType>()
        {
            return this.And(typeof(TType));
        }

        public void As(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            if (typeImplementation.IsAbstract || typeImplementation.IsInterface)
            {
                throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            InversionOfControlResources.ErrMsg_CannotSetInterfaceAsImplementation,
                            this.implementation));
            }

            foreach (var registeredType in this.registeredTypes)
            {
                if (!registeredType.IsAssignableFrom(typeImplementation))
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            InversionOfControlResources.ErrMsg_CannotSetTypeAsImplementation,
                            typeImplementation,
                            registeredType),
                        "typeImplementation");
                }
            }

            lock (this.typeLocker)
            {
                this.CheckState();

                this.implementation = typeImplementation;
            }
        }

        public void As<TType>()
        {
            this.As(typeof(TType));
        }

        public void As(Func<object> factoryFunction)
        {
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");
            }

            this.As((arguments) => factoryFunction());
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
                this.isSingleton = true;
            }
        }

        public void AsSingleton()
        {
            lock (this.typeLocker)
            {
                if (this.registeredTypes.Count != 1)
                {
                    throw new NotSupportedException(InversionOfControlResources.ErrMsg_CannotRegisterTypeAsSingleton);
                }

                this.AsSingleton(this.registeredTypes[0]);
            }
        }

        public void AsSingleton<TType>()
        {
            this.AsSingleton(typeof(TType));
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
                this.isSingleton = true;
            }
        }

        public void AsSingleton(Func<object> factoryFunction)
        {
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");
            }

            this.AsSingleton((arguments) => factoryFunction());
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
                this.isSingleton = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal object Resolve(object[] arguments = null)
        {
            this.CheckDisposed();

            if (this.instance != null)
            {
                Debug.Assert(this.isSingleton, "this.isSingleton");
                return this.instance;
            }

            try
            {
                if (this.isSingleton)
                {
                    Monitor.Enter(this.typeLocker);
                }

                if (this.instance != null)
                {
                    Debug.Assert(this.isSingleton, "this.isSingleton");
                    return this.instance;
                }

                // If we don't have a factory and we don't initialized this.requiredInjections
                // we need to find a type which can construct objects and compile function
                // which can create new instances of this type.
                if (this.factory == null || this.requiredInjections == null)
                {
                    if (!this.isSingleton)
                    {
                        Monitor.Enter(this.typeLocker);
                    }

                    if (this.factory == null || this.requiredInjections == null)
                    {
                        this.Compile();
                    }

                    if (!this.isSingleton)
                    {
                        Monitor.Exit(this.typeLocker);
                    }
                }

                if (this.factory == null)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, InversionOfControlResources.ErrMsg_CannotFindConstructorForType, this.implementation));
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
                                    if (p.ContainerInstance == null)
                                    {
                                        value = this.container.Resolve(p.Type, arguments);
                                        p.ContainerInstance = this.container.Get(p.Type);
                                    }
                                    else
                                    {
                                        value = p.ContainerInstance.Resolve(arguments);
                                    }
                                }

                                return value;
                            }).ToArray();
                }
                else
                {
                    ctorArguments = arguments;
                }

                object result = this.factory(ctorArguments);
                    
                if (this.isSingleton)
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
                if (this.isSingleton)
                {
                    Monitor.Exit(this.typeLocker);
                }
            }
        }

        private void Compile()
        {
            if (this.factory == null && this.instance == null)
            {
                if (this.implementation == null)
                {
                    if (this.registeredTypes.Count > 1)
                    {
                        throw new NotSupportedException(InversionOfControlResources.ErrMsg_MoreThanTwoTypesShouldHaveImplementationType);
                    }

                    this.implementation = this.registeredTypes.First();

                    if (this.implementation.IsAbstract || this.implementation.IsInterface)
                    {
                        throw new NotSupportedException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                InversionOfControlResources.ErrMsg_ImplementationIsNotSpecifiedForInterface,
                                this.implementation));
                    }
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
            this.CheckDisposed();
            this.CheckBehavior();
        }

        private void CheckBehavior()
        {
            if (this.factory != null || this.implementation != null || this.instance != null)
            {
                throw new NotSupportedException(InversionOfControlResources.ErrMsg_CannotSetMoreThanOneBehavior);
            }
        }

        private void Dispose(bool disposing)
        {
            lock (this.typeLocker)
            {
                if (!this.isDisposed)
                {
                    if (disposing)
                    {
                        this.factory = null;

                        var disposable = this.instance as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }

                        if (this.requiredInjections != null)
                        {
                            this.requiredInjections.Clear();
                            this.requiredInjections = null;
                        }
                    }

                    this.isDisposed = true;
                }
            }
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(ContainerInstance).Name);
            }
        }

        private class InjectInfo
        {
            public InjectInfo(Type type, ContainerInstance containerInstance)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }

                this.Type = type;
                this.ContainerInstance = containerInstance;
            }

            public Type Type { get; private set; }

            public ContainerInstance ContainerInstance { get; set; }
        }
    }
}