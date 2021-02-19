using System;
using System.Text.Json.Serialization;

namespace Hexarc.Pact.Protocol.Api
{
    public class Controller
    {
        public String? Namespace { get; }

        public String Name { get; }

        public String Path { get; }

        public Method[] Methods { get; }

        /// <summary>
        /// Gets the full type name.
        /// </summary>
        [JsonIgnore]
        public String FullName => String.IsNullOrEmpty(this.Namespace) ? this.Name : $"{this.Namespace}.{this.Name}";

        public Controller(String? @namespace, String name, String path, Method[] methods)
        {
            this.Namespace = @namespace;
            this.Name = name;
            this.Path = path;
            this.Methods = methods;
        }
    }
}
