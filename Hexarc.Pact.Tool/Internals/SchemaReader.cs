using Hexarc.Pact.Protocol.Api;

namespace Hexarc.Pact.Tool.Internals;

public sealed class SchemaReader
{
    private JsonSerializerOptions JsonSerializerOptions { get; }

    private HttpClient HttpClient { get; } = new();

    public SchemaReader(JsonSerializerOptions jsonSerializerOptions) =>
        this.JsonSerializerOptions = jsonSerializerOptions;

    public async Task<Schema?> ReadAsync(String schemaUri, String[]? scopes) =>
        await this.HttpClient.GetFromJsonAsync<Schema>(
            this.BuildUri(schemaUri, scopes),
            this.JsonSerializerOptions);

    private Uri BuildUri(String schemaUri, String[]? scopes) =>
        scopes is null ?
            new Uri(schemaUri) :
            new UriBuilder(schemaUri) { Query = this.ToScopeQuery(scopes) }.Uri;

    private String ToScopeQuery(String[] scopes) =>
        String.Join("&", scopes.Select(x => $"scope={x}"));
}
