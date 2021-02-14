using System;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class ClientSettings
    {
        public String SchemaUrl { get; }

        public String ClientClassName { get; }

        public String? ClientClassNamespace { get; }

        public String OutputDirectory { get; }

        public ClientSettings(String schemaUrl, String clientClassName, String? clientClassNamespace, String outputDirectory)
        {
            this.SchemaUrl = schemaUrl;
            this.ClientClassName = clientClassName;
            this.ClientClassNamespace = clientClassNamespace;
            this.OutputDirectory = outputDirectory;
        }
    }
}
