using System;

namespace Hexarc.Pact.Protocol.Api
{
    public class Controller
    {
        public String? Namespace { get; }

        public String Name { get; }

        public String Path { get; }

        public Method[] Methods { get; }

        public Controller(String? @namespace, String name, String path, Method[] methods)
        {
            this.Namespace = @namespace;
            this.Name = name;
            this.Path = path;
            this.Methods = methods;
        }
    }
}
