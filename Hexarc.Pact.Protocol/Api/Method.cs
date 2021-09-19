using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Api;

/// <summary>
/// Contains information about an API endpoint method.
/// </summary>
public sealed class Method
{
    /// <summary>
    /// Gets the method name.
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// Gets the method path.
    /// </summary>
    public String Path { get; }

    /// <summary>
    /// Gets the method HTTP verb.
    /// </summary>
    public HttpMethod HttpMethod { get; }

    /// <summary>
    /// Gets the method return type.
    /// </summary>
    public TaskTypeReference ReturnType { get; }

    /// <summary>
    /// Gets the method parameters.
    /// </summary>
    public MethodParameter[] Parameters { get; }

    /// <summary>
    /// Creates an instance of the Method class.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="path">The method path.</param>
    /// <param name="httpMethod">The method's HTTP verb.</param>
    /// <param name="returnType">The method return type.</param>
    /// <param name="parameters">The method parameters.</param>
    public Method(
        String name,
        String path,
        HttpMethod httpMethod,
        TaskTypeReference returnType,
        MethodParameter[] parameters)
    {
        this.Name = name;
        this.Path = path;
        this.HttpMethod = httpMethod;
        this.ReturnType = returnType;
        this.Parameters = parameters;
    }
}
