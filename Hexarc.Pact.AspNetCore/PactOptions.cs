using System.Reflection;
using Hexarc.Pact.AspNetCore.Internals;

namespace Hexarc.Pact.AspNetCore
{
    public sealed class PactOptions
    {
        public Assembly AssemblyWithControllers { get; set; }

        public TypeProvider? TypeProvider { get; set; }

        public PactOptions() : this(Assembly.GetEntryAssembly()!) { }

        public PactOptions(Assembly assemblyWithControllers) =>
            this.AssemblyWithControllers = assemblyWithControllers;
    }
}
