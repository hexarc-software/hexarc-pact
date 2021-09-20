﻿namespace Hexarc.Pact.Client;

using Hexarc.Serialization.Tuple;
using Hexarc.Serialization.Union;

public abstract class ClientBase
{
    public HttpClient HttpClient { get; }

    public Uri BaseUri => this.HttpClient.BaseAddress ??
                            throw new NullReferenceException("No base path provided in the HTTP client");

    public JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new UnionConverterFactory(), new TupleConverterFactory() }
    };

    protected ClientBase(HttpClient httpClient) =>
        this.HttpClient = httpClient;
}
