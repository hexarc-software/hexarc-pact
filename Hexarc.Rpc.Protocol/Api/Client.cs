using System;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Client
    {
        public String Namespace { get; }

        public String Name { get; }

        public String BaseAddress { get; }

        public Controller[] Controllers { get; }

        public Client(String @namespace, String name, String baseAddress, Controller[] controllers)
        {
            this.Namespace = @namespace;
            this.Name = name;
            this.BaseAddress = baseAddress;
            this.Controllers = controllers;
        }
    }
}
