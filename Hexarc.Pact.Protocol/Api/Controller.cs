namespace Hexarc.Pact.Protocol.Api;

/// <summary>
/// Provides information about an API controller type and it's endpoints.
/// </summary>
public sealed class Controller
{
    /// <summary>
    /// Gets the controller type namespace.
    /// </summary>
    public String? Namespace { get; }

    /// <summary>
    /// Gets the controller type name.
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// Gets the controller root path.
    /// </summary>
    public String Path { get; }

    /// <summary>
    /// Gets the controller's endpoints.
    /// </summary>
    public Method[] Methods { get; }

    /// <summary>
    /// Gets the type full name.
    /// </summary>
    [JsonIgnore]
    public String FullName => String.IsNullOrEmpty(this.Namespace) ? this.Name : $"{this.Namespace}.{this.Name}";

    /// <summary>
    /// Creates an instance of the Controller class.
    /// </summary>
    /// <param name="namespace">The namespace of the provided API Controller type.</param>
    /// <param name="name">The name of the API Controller type.</param>
    /// <param name="path">The root path of the API Controller.</param>
    /// <param name="methods">The controller's endpoint methods.</param>
    public Controller(String? @namespace, String name, String path, Method[] methods)
    {
        this.Namespace = @namespace;
        this.Name = name;
        this.Path = path;
        this.Methods = methods;
    }
}
