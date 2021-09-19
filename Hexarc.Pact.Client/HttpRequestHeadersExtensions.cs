namespace Hexarc.Pact.Client;

internal static class HttpRequestHeadersExtensions
{
    public static void AddRange(this HttpHeaders httpHeaders, IEnumerable<KeyValuePair<String, String>> headers)
    {
        foreach (var (key, value) in headers)
        {
            httpHeaders.Add(key, value);
        }
    }
}
