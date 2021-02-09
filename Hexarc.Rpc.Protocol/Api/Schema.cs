using Hexarc.Rpc.Protocol.Types;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Schema
    {
        public Controller[] Controllers { get; }

        public DistinctType[] DistinctTypes { get; }

        public Schema(Controller[] controllers, DistinctType[] distinctTypes) =>
            (this.Controllers, this.DistinctTypes) = (controllers, distinctTypes);
    }
}
