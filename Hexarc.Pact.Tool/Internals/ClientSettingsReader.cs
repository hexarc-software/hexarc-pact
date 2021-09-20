namespace Hexarc.Pact.Tool.Internals;

using Hexarc.Pact.Tool.Extensions;
using Hexarc.Pact.Tool.Models;

public sealed class ClientSettingsReader
{
    private const String SettingsFileName = "pact.json";

    private JsonSerializerOptions JsonSerializerOptions { get; }

    public ClientSettingsReader(JsonSerializerOptions jsonSerializerOptions) =>
        this.JsonSerializerOptions = jsonSerializerOptions;

    public async Task<IEnumerable<ClientSettings>?> Read()
    {
        if (!File.Exists(SettingsFileName)) return default;

        await using var stream = File.OpenRead(SettingsFileName);
        using var document = await JsonDocument.ParseAsync(stream);

        return document.RootElement.ValueKind switch
        {
            JsonValueKind.Array => document.ToObject<ClientSettings[]>(this.JsonSerializerOptions),
            _ => new[] { document.ToObject<ClientSettings>(this.JsonSerializerOptions)! }
        };
    }
}
