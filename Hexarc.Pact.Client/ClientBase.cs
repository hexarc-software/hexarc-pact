using System;
using System.Net.Http;
using System.Text.Json;

namespace Hexarc.Pact.Client
{
    public abstract class ClientBase
    {
        public HttpClient HttpClient { get; }

        public Uri BaseUri { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        protected ClientBase(HttpClient httpClient, Uri baseUri) =>
            (this.HttpClient, this.BaseUri) = (httpClient, baseUri);
    }
}
