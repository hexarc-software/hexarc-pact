using System;
using System.Reflection;

namespace Hexarc.Rpc.Server
{
    public sealed class HexarcRpcOptions
    {
        public String Route { get; set; } = "hexarc/schema";

        public Assembly AssemblyWithControllers { get; set; }

        public HexarcRpcOptions() : this(Assembly.GetEntryAssembly()!) { }

        public HexarcRpcOptions(Assembly assemblyWithControllers) =>
            this.AssemblyWithControllers = assemblyWithControllers;
    }
}
