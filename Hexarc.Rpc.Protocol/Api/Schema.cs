using Hexarc.Rpc.Protocol.Types;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Schema
    {
        public Client Client { get; }

        public DistinctType[] DistinctTypes { get; }

        public Schema(Client client, DistinctType[] distinctTypes) =>
            (this.Client, this.DistinctTypes) = (client, distinctTypes);
    }
}
