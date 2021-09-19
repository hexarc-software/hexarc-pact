namespace Hexarc.Pact.Protocol.Api;

/// <summary>
/// Contains supported HTTP methods of the Hexarc Pact protocol.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpMethod
{
    /// <summary>
    /// The http GET method.
    /// </summary>
    Get = 1,

    /// <summary>
    /// The http POST method.
    /// </summary>
    Post = 2
}
