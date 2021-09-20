namespace Hexarc.Pact.AspNetCore;

using Hexarc.Pact.AspNetCore.Internals;

public sealed class PactOptions
{
    public Assembly AssemblyWithControllers { get; set; }

    public TypeProvider? TypeProvider { get; set; }

    public PactOptions() : this(Assembly.GetEntryAssembly()!) { }

    public PactOptions(Assembly assemblyWithControllers) =>
        this.AssemblyWithControllers = assemblyWithControllers;
}
