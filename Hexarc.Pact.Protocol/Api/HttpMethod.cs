using System.Text.Json.Serialization;

namespace Hexarc.Pact.Protocol.Api
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HttpMethod
    {
        Get = 1,
        Post = 2
    }
}
