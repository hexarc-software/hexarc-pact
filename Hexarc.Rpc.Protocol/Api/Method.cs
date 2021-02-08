using System;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Method
    {
        public String Name { get; }

        public Method(String name)
        {
            this.Name = name;
        }
    }
}
