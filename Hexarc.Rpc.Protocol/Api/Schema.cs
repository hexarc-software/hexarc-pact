using Hexarc.Rpc.Protocol.Types;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Schema
    {
        public Controller[] Controllers { get; }

        public Type[] DistinctTypes { get; }

        public Schema(Controller[] controllers, Type[] distinctTypes) =>
            (this.Controllers, this.DistinctTypes) = (controllers, distinctTypes);
    }
}
