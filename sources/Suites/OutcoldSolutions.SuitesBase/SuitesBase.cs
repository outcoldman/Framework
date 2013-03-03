// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    using NUnit.Framework;

    using OutcoldSolutions.Diagnostics;

    public abstract class SuitesBase
    {
        private readonly IDebugConsole debugConsole;

        protected SuitesBase(IDebugConsole debugConsole)
        {
            if (debugConsole == null)
            {
                throw new ArgumentNullException("debugConsole");
            }

            this.debugConsole = debugConsole;
        }

        protected IDependencyResolverContainer Container { get; private set; }

        protected ILogManager LogManager { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Container = new DependencyResolverContainer();

            ApplicationBase.Container = this.Container;

            using (var registration = this.Container.Registration())
            {
                registration.Register<ILogManager>().AsSingleton<LogManager>();
                registration.Register<IDebugConsole>().AsSingleton(this.debugConsole);
            }

            this.LogManager = this.Container.Resolve<ILogManager>();
            this.LogManager.LogLevel = LogLevel.Info;
            this.LogManager.Writers.AddOrUpdate(typeof(DebugLogWriter), type => new DebugLogWriter(this.Container), (type, writer) => writer);
        }
    }
}