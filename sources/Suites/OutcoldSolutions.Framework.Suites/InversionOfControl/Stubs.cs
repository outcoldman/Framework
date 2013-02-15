// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------


namespace OutcoldSolutions.Framework.InversionOfControl
{
    public interface IServiceStubBase
    {
    }

    public interface IServiceStub1 : IServiceStubBase
    {
    }

    public interface IServiceStubParent1 : IServiceStub1
    {
    }

    public interface IServiceStub2 : IServiceStubBase
    {
    }

    public class ServiceStub : IServiceStubParent1, IServiceStub2
    {
    }

    public class ServiceWithConstructorStub : IServiceStubParent1, IServiceStub2
    {
        public ServiceWithConstructorStub(string a, int b)
        {
            this.A = a;
            this.B = b;
        }

        public string A { get; set; }

        public int B { get; set; }
    }

    public class ServiceWithConstructorsStub : IServiceStubParent1, IServiceStub2
    {
        [Inject]
        public ServiceWithConstructorsStub(string a)
        {
            this.A = a;
        }

        public ServiceWithConstructorsStub(string b, int i)
        {
            this.B = b;
        }

        public string A { get; set; }

        public string B { get; set; }
    }

    public class ServiceCircularStub : IServiceStubParent1, IServiceStub2
    {
        private readonly ServiceCircularStub child;

        public ServiceCircularStub(ServiceCircularStub child)
        {
            this.child = child;
        }
    }

    public class ServiceWithoutInjectAttributeStub 
    {
        public ServiceWithoutInjectAttributeStub(IServiceStub1 child)
        {
        }

        public ServiceWithoutInjectAttributeStub(IServiceStub2 child)
        {
        }
    }

    public class ServiceWithDependencyStub
    {
        public ServiceWithDependencyStub(IServiceStub1 child)
        {
            this.Child = child;
        }

        public IServiceStub1 Child { get; set; }
    }

    public class ServiceWithMethodInjection
    {
        public ServiceWithMethodInjection(IServiceStub1 child)
        {
            this.Child = child;
        }

        public IServiceStub1 Child { get; set; }

        public IServiceStub2 Child2 { get; set; }

        [Inject]
        public void MethodA(IServiceStub2 serviceStub2)
        {
            this.Child2 = serviceStub2;
        }
    }

    public class ServiceWithMethodWithReturnInjection
    {
        public IServiceStub1 Child { get; set; }

        [Inject]
        public IServiceStub1 MethodA(IServiceStub1 serviceStub2)
        {
            return this.Child = serviceStub2;
        }
    }

    public class ServiceWithPrivateMethodInjection
    {
        public IServiceStub1 Child { get; set; }

        [Inject]
        private void MethodA(IServiceStub1 serviceStub1)
        {
            this.Child = serviceStub1;
        }
    }

    public class ServiceWithStubBaseInjection
    {
        public ServiceWithStubBaseInjection(IServiceStubBase stubBase)
        {
            this.Child = stubBase;
        }

        public IServiceStubBase Child { get; set; }
    }
}