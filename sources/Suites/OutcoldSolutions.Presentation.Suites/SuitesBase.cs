// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Suites
{
    using NUnit.Framework;

    using OutcoldSolutions.Presentation.Diagnostics;

    public abstract class SuitesBase
    {
        protected IDependencyResolverContainer Container { get; private set; }

        protected ILogManager LogManager { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Container = new DependencyResolverContainer();

            using (var registration = this.Container.Registration())
            {
                registration.Register<ILogManager>().AsSingleton<LogManager>();
            }

            this.LogManager = this.Container.Resolve<ILogManager>();
            this.LogManager.LogLevel = LogLevel.Info;
            this.LogManager.Writers.AddOrUpdate(typeof(DebugLogWriter), type => new DebugLogWriter(), (type, writer) => writer);
        }
    }
}