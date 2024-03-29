namespace Hexarc.Pact.Tool.Models;

public sealed class ClientSettings
{
    public String SchemaUri { get; }

    public String ClientClassName { get; }

    public String? ClientClassNamespace { get; }

    public String[]? Scopes { get; }

    public String OutputDirectory { get; }

    public GenerationOptions? GenerationOptions { get; }

    public ClientSettings(
        String schemaUri,
        String clientClassName,
        String? clientClassNamespace,
        String[]? scopes,
        String outputDirectory,
        GenerationOptions? generationOptions)
    {
        this.SchemaUri = schemaUri;
        this.ClientClassName = clientClassName;
        this.ClientClassNamespace = clientClassNamespace;
        this.Scopes = scopes;
        this.OutputDirectory = outputDirectory;
        this.GenerationOptions = generationOptions;
    }
}
