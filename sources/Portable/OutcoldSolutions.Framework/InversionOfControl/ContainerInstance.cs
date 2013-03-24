// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
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
        private const string CompiledExpressionInstanceName = "i";
        private static readonly Type InjectAttributeType = typeof(InjectAttribute);

        private readonly DependencyResolverContainer container;
        private readonly List<Type> registeredTypes = new List<Type>();
        private readonly object typeLocker = new object();
        
        private Type implementation;
        private bool isSingleton;
        private Func<object[], object> factory;
        private List<MethodInjectInfo> methodInjections;
        private object instance;

        private Dictionary<Type, Type> injectionRules;

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

        public IContainerInstance InjectionRule(Type type, Type typeImplementation)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            if (!type.GetTypeInfo().IsAssignableFrom(typeImplementation.GetTypeInfo()))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        InversionOfControlResources.ErrMsg_CannotSetTypeAsImplementation,
                        typeImplementation,
                        type),
                    "typeImplementation");
            }

            if (this.injectionRules == null)
            {
                this.injectionRules = new Dictionary<Type, Type>();
            }

            if (this.injectionRules.ContainsKey(type))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        InversionOfControlResources.ErrMsg_CannotSetInjectionTypeTwice,
                        type),
                    "type");
            }

            this.injectionRules.Add(type, typeImplementation);

            return this;
        }

        public IContainerInstance InjectionRule<TType, TImplementation>() where TImplementation : TType
        {
            return this.InjectionRule(typeof(TType), typeof(TImplementation));
        }

        public void As(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            TypeInfo typeInfo = typeImplementation.GetTypeInfo();
            if (typeInfo.IsAbstract || typeInfo.IsInterface)
            {
                throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            InversionOfControlResources.ErrMsg_CannotSetInterfaceAsImplementation,
                            this.implementation));
            }

            foreach (var registeredType in this.registeredTypes)
            {
                if (!registeredType.GetTypeInfo().IsAssignableFrom(typeInfo))
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

                    try
                    {
                        if (this.factory == null || this.requiredInjections == null)
                        {
                            this.Compile();
                        }
                    }
                    finally 
                    {
                        if (!this.isSingleton)
                        {
                            Monitor.Exit(this.typeLocker);
                        }
                    }
                }

                if (this.factory == null)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, InversionOfControlResources.ErrMsg_CannotFindConstructorForType, this.implementation));
                }

                object[] ctorArguments = this.ResolveRequiredParameters(this.requiredInjections, arguments);
                object result = this.factory(ctorArguments);
                if (this.methodInjections != null)
                {
                    foreach (var methodInjectInfo in this.methodInjections)
                    {
                        methodInjectInfo.FuncCall(result, this.ResolveRequiredParameters(methodInjectInfo.RequiredInjections, arguments));
                    }
                }
                    
                if (this.isSingleton)
                {
                    // In case if this is singleton we will remember value
                    // and will clear all unnecessary objects.
                    this.instance = result;
                    this.factory = null;
                    this.requiredInjections = null;
                    this.methodInjections = null;
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

        private object[] ResolveRequiredParameters(IEnumerable<InjectInfo> injectInfos, object[] arguments)
        {
            if (injectInfos == null)
            {
                return null;
            }

            return injectInfos.Select(
                        p =>
                            {
                                object value = null;
                                if (arguments != null)
                                {
                                    TypeInfo typeInfo = p.Type.GetTypeInfo();
                                    value = arguments.FirstOrDefault(a =>
                                        {
                                            if (a != null)
                                            {
                                                return typeInfo.IsAssignableFrom(a.GetType().GetTypeInfo());
                                            }

                                            return false;
                                        });
                                }

                                if (value == null)
                                {
                                    if (p.ContainerInstance == null)
                                    {
                                        p.ContainerInstance = this.container.Get(p.Type);
                                    }

                                    if (p.ContainerInstance == null)
                                    {
                                        throw new InvalidOperationException(
                                            string.Format(
                                                CultureInfo.CurrentCulture, 
                                                InversionOfControlResources.ErrMsg_CannotResolveParameterType,
                                                p.Type));
                                    }

                                    value = p.ContainerInstance.Resolve(arguments);
                                }

                                return value;
                            }).ToArray();
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

                    TypeInfo typeInfo = this.implementation.GetTypeInfo();
                    if (typeInfo.IsAbstract || typeInfo.IsInterface)
                    {
                        throw new NotSupportedException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                InversionOfControlResources.ErrMsg_ImplementationIsNotSpecifiedForInterface,
                                this.implementation));
                    }
                }

                ConstructorInfo[] constructorInfos = this.implementation.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic).ToArray();
                if (constructorInfos.Length > 1)
                {
                    constructorInfos = constructorInfos.Where(info =>
                    {
                        IEnumerable<Attribute> customAttributes = info.GetCustomAttributes(InjectAttributeType, false);
                        return customAttributes != null && customAttributes.Any();
                    }).ToArray();
                }

                // If we have more than 1 constructor we will not compile function
                // and will throw exception because this.factory will be null.
                if (constructorInfos.Length == 1)
                {
                    var constructorInfo = constructorInfos[0];

                    var parameterTypes = constructorInfo.GetParameters().Select(info => info.ParameterType).ToList();
                    this.requiredInjections = this.GetInjectInfos(parameterTypes).ToList();

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
                
                this.methodInjections = this.implementation.GetRuntimeMethods()
                    .Select(method => new { Method = method, Attributes = method.GetCustomAttributes(InjectAttributeType, inherit: true) })
                    .Where(method => method.Attributes != null && method.Attributes.Any())
                    .Select(method =>
                    {
                        var methodInfo = method.Method;
                        var parameterTypes = methodInfo.GetParameters().Select(info => info.ParameterType).ToList();
                        var methodRequiredInjections = this.GetInjectInfos(parameterTypes).ToList();

                        // Compile a new lambda expression:
                        // (i, (object[]) a) => 
                        // {
                        //    return i.Method((TType0)a[0], (TType1)a[1], ... , (TTypeN)a[N]);
                        // }
                        ParameterExpression instanceExpression = Expression.Parameter(typeof(object), CompiledExpressionInstanceName);
                        ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), CompiledExpressionInputParameterName);
                        var expressions = parameterTypes
                            .Select((t, i) => Expression.Convert(Expression.ArrayIndex(parameterExpression, Expression.Constant(i)), t))
                            .Cast<Expression>().ToList();

                        MethodCallExpression callExp = Expression.Call(Expression.Convert(instanceExpression, this.implementation), methodInfo, expressions);
                        LambdaExpression lambda = Expression.Lambda(callExp, instanceExpression, parameterExpression);
                        var @delegate = lambda.Compile();

                        Action<object, object[]> action = @delegate as Action<object, object[]>;
                        if (action == null)
                        {
                            action = (object i, object[] a) => ((Func<object, object[], object>)@delegate)(i, a);
                        }

                        return new MethodInjectInfo(action, methodRequiredInjections);
                    }).ToList();

                if (this.methodInjections.Count == 0)
                {
                    this.methodInjections = null;
                }

                this.injectionRules = null;
            }
        }

        private IEnumerable<InjectInfo> GetInjectInfos(IEnumerable<Type> parameterTypes)
        {
            return parameterTypes.Select(t =>
            {
                Type projection;
                if (this.injectionRules != null 
                    && this.injectionRules.TryGetValue(t, out projection))
                {
                    t = projection;
                }

                return new InjectInfo(t, this.container.Get(t));
            });
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

        private class MethodInjectInfo
        {
            public MethodInjectInfo(Action<object, object[]> funcCall, List<InjectInfo> requiredParameters)
            {
                this.RequiredInjections = requiredParameters;
                this.FuncCall = funcCall;
            }

            public List<InjectInfo> RequiredInjections { get; private set; }

            public Action<object, object[]> FuncCall { get; private set; }
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