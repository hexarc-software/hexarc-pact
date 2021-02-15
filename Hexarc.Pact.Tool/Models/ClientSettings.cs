using System;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class ClientSettings
    {
        public String SchemaUri { get; }

        public String ClientClassName { get; }

        public String? ClientClassNamespace { get; }

        public String OutputDirectory { get; }

        public ClientSettings(String schemaUri, String clientClassName, String? clientClassNamespace, String outputDirectory)
        {
            this.SchemaUri = schemaUri;
            this.ClientClassName = clientClassName;
            this.ClientClassNamespace = clientClassNamespace;
            this.OutputDirectory = outputDirectory;
        }
    }
}
