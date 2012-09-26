// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------


namespace OutcoldSolutions.Framework.InversionOfControl
{
    public interface IServiceStub1
    {
    }


    public interface IServiceStub2
    {
    }

    public class ServiceStub : IServiceStub1, IServiceStub2
    {
    }

    public class ServiceWithConstructorStub : IServiceStub1, IServiceStub2
    {
        public ServiceWithConstructorStub(string a, int b)
        {
            this.A = a;
            this.B = b;
        }

        public string A { get; set; }

        public int B { get; set; }
    }

    public class ServiceWithConstructorsStub : IServiceStub1, IServiceStub2
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

    public class ServiceCircularStub : IServiceStub1, IServiceStub2
    {
        private readonly ServiceCircularStub child;

        public ServiceCircularStub(ServiceCircularStub child)
        {
            this.child = child;
        }
    }
}