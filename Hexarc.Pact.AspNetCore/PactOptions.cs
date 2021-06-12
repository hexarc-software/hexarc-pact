using System.Reflection;

namespace Hexarc.Pact.AspNetCore
{
    public sealed class PactOptions
    {
        public Assembly AssemblyWithControllers { get; set; }

        public PactOptions() : this(Assembly.GetEntryAssembly()!) { }

        public PactOptions(Assembly assemblyWithControllers) =>
            this.AssemblyWithControllers = assemblyWithControllers;
    }
}
