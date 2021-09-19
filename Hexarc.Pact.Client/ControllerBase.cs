namespace Hexarc.Pact.Client;

public abstract class ControllerBase
{
    protected ClientBase Client { get; }

    protected String ControllerPath { get; }

    protected Uri BaseUri => this.Client.BaseUri;

    protected HttpClient HttpClient => this.Client.HttpClient;

    protected JsonSerializerOptions JsonSerializerOptions => this.Client.JsonSerializerOptions;

    protected ControllerBase(ClientBase client, String controllerPath)
    {
        this.Client = client;
        this.ControllerPath = controllerPath;
    }

    protected async Task DoGetRequestWithVoidResponse(
        String methodPath,
        IEnumerable<GetMethodParameter> parameters,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath, parameters);
        var message = new HttpRequestMessage { RequestUri = uri, Method = HttpMethod.Get };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
    }

    protected async Task<TResponse> DoGetRequestWithJsonResponse<TResponse>(
        String methodPath,
        IEnumerable<GetMethodParameter> parameters,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath, parameters);
        var message = new HttpRequestMessage { RequestUri = uri, Method = HttpMethod.Get };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    protected async Task DoPostVoidRequestWithVoidResponse(
        String methodPath,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath);
        var message = new HttpRequestMessage { RequestUri = uri, Method = HttpMethod.Post };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
    }

    protected async Task<TResponse> DoPostVoidRequestWithJsonResponse<TResponse>(
        String methodPath,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath);
        var message = new HttpRequestMessage { RequestUri = uri, Method = HttpMethod.Post };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    protected async Task DoPostJsonRequestWithVoidResponse<TRequest>(
        String methodPath,
        TRequest request,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath);
        var data = JsonSerializer.Serialize(request, this.JsonSerializerOptions);
        var content = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        var message = new HttpRequestMessage { RequestUri = uri, Content = content, Method = HttpMethod.Post };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
    }

    protected async Task<TResponse> DoPostJsonRequestWithJsonResponse<TRequest, TResponse>(
        String methodPath,
        TRequest request,
        IEnumerable<KeyValuePair<String, String>>? headers = default)
    {
        var uri = this.ToMethodUri(methodPath);
        var data = JsonSerializer.Serialize(request, this.JsonSerializerOptions);
        var content = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        var message = new HttpRequestMessage { RequestUri = uri, Content = content, Method = HttpMethod.Post };
        if (headers is not null) message.Headers.AddRange(headers);
        var response = await this.HttpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"API endpoint `{uri}` failed with code {response.StatusCode}");
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    protected Uri ToMethodUri(String methodPath) => new(this.BaseUri, $"{this.ControllerPath}{methodPath}");

    protected Uri ToMethodUri(String methodPath, IEnumerable<GetMethodParameter> parameters)
    {
        var pairs = parameters.Where(x => x.Value is not null).Select(x => x.QueryStringKeyValue);
        var query = String.Join("&", pairs);
        return new Uri(this.BaseUri, $"{this.ControllerPath}{methodPath}?{query}");
    }
}
