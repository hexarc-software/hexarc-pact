using System;

namespace Hexarc.Rpc.Protocol.Api
{
    public class Controller
    {
        public String Namespace { get; }

        public String Name { get; }

        public Method[] Methods { get; }

        public Controller(String @namespace, String name, Method[] methods)
        {
            this.Namespace = @namespace;
            this.Name = name;
            this.Methods = methods;
        }
    }
}
