using System;
using System.Collections.Generic;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Schema
    {
        public Client Client { get; }

        public Dictionary<Guid, Type> Types { get; }

        public Schema(Client client, Dictionary<Guid, Type> types)
        {
            this.Client = client;
            this.Types = types;
        }
    }
}
