# Hexarc Pact
Hexarc Pact provides a toolchain for exposing and consuming Web API for .NET/TypeScript-based projects.

Develop your .NET Web API project with Pact in mind and consume it across the .NET ecosystem 
(microservices, desktop and mobile applications) or on the Web via TypeScript.

|Package|Platform|Version|Downloads|
|-------|--------|-------|---------|
|`Hexarch.Pact.Protocol`| .NET 5.0+ | [![Version](https://img.shields.io/nuget/v/Hexarc.Pact.Protocol.svg)](https://nuget.org/packages/Hexarc.Pact.Protocol) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.Protocol.svg)](https://nuget.org/packages/Hexarc.Pact.Protocol) |
|`Hexarch.Pact.AspNetCore`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.AspNetCore.svg)](https://nuget.org/packages/Hexarc.Pact.AspNetCore) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.AspNetCore.svg)](https://nuget.org/packages/Hexarc.Pact.AspNetCore) |
|`Hexarch.Pact.Client`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.Client.svg)](https://nuget.org/packages/Hexarc.Pact.Client) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.Client.svg)](https://nuget.org/packages/Hexarc.Pact.Client) |
|`Hexarch.Pact.Tool`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.Tool.svg)](https://nuget.org/packages/Hexarc.Pact.Tool) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.Tool.svg)](https://nuget.org/packages/Hexarc.Pact.Tool) |
|`@hexarc/pact-tool`| TypeScript | [![Version](http://img.shields.io/npm/v/@hexarc/pact-tool.svg)](https://www.npmjs.org/package/@hexarc/pact-tool) | [![Downloads](http://img.shields.io/npm/dt/@hexarc/pact-tool.svg)](https://www.npmjs.org/package/@hexarc/pact-tool) |

## What's in Pact?
* [Features](#features)
* [Type system](#type-system)
* [API annotation rules](#api-annotation-rules)
  * [Additional attributes](#additional-attributes)
* [Demo API](#demo-api-server)
* [How to use](#how-to-use)
  * [Expose Pact API schema](#expose-pact-api-schema) (.NET Web API service)
  * [Consume Pact API schema](#consume-pact-api-schema) (.NET client application)
* [License](#license)

## Features
* Hassle-free API integrations in seconds.
* No type information loosing. Consume exact API that is provided.
* Code first. No need to describe your API protocol in external IDL/DSL.
* Charged with advanced types. Not only simple DTOs but enums, generics, tagged unions, etc.

## Type system
The Pact type system is based on the .NET CLR types with some bespoke ones
for the best development experience. 

What's inside the Pact type system:
* Primitive types (including `Guid` and `DateTime`)
* Value/Reference type semantics
* Collections and generics (not only simple arrays and dictionaries)
* Complete support for Nullable Reference Type annotations (NRT)
* Enums (string-based and number-based)
* Tagged Unions (via [Hexarc.Serialization.Union](https://github.com/hexarc-software/hexarc-serialization))
* Tuples (via [Hexarc.Serialization.Tuple](https://github.com/hexarc-software/hexarc-serialization))

## API annotation rules
The Pact API schema is designed to have an RPC-like semantic. In doing so 
an exposed API must follow these rules:
* API controllers must be marked with standard `ApiController` and `Route` attributes.
* Route attribute must have only a constant string path without templating (e.g. `[Route("Entities")]`).
* API methods must be marked with one of the supported HTTP verbs (`HttpGet` or `HttpPost`). Verb attributes may contain an endpoint path. 
  Others are not supported at the moment. 
   * `HttpGet` methods can have query parameters which must be bind via the `FromQuery` attribute.
   * `HttpPost` methods must have only one parameter which is the request body.
* API method path can be specified in the HTTP verb attribute or the `Route` attribute. If the method path is not
specified in the HTTP verb attribute it will be taken from the `Route` one or an exception will be raised.

### Additional attributes
Pact provides some useful attributes for API annotation:
* `PactIgnoreAttribute` can be applied to API controllers or methods to be excluded from an API schema.
* `PactScopeAttribute` can be applied to API controllers to be isolated into specific scope in an API schema.

## Demo API server
Demo API server can be found at https://hexarc-demo-api.herokuapp.com/.

The Pact API schema is exposed at https://hexarc-demo-api.herokuapp.com/pact/schema.

To generate an API client use the [instruction](#consume-pact-api-schema)
below.

## How to use
Find out how to use Pact to expose and consume a typical Web API.

### Expose Pact API schema
1. Install the `Hexarc.Pact.AspNetCore` package in a .NET Web API project:
```shell
dotnet add package Hexarc.Pact.AspNetCore
```
2. Add the Pact schema generation in the `Startup.ConfigureServices` method:
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // Register the Pact schema generation.
    services.AddPactGeneration();
}
```
3. Enable the Pact middleware in the `Startup.Configure` method:
```c#
public void Configure(IApplicationBuilder app)
{
    // Enable the Pact middleware to expose the generated API schema.
    app.UsePact();

    app.UseRouting();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
}
```
4. Implement an API controller according to the Pact annotation rules:
```c#
[ApiController, Route("Misc")]
public sealed class MiscController : ControllerBase
{
    [HttpGet(nameof(Ping))]
    public String Ping([FromQuery] String message) => $"Hello, {message}";
}
```
5. Start the app and open the `YOUR_API_HOST/pact/schema` address to ensure 
   the Pact API schema is generated.
   
### Consume Pact API schema
1. Install the `Hexarc.Pact.Client` package in a .NET project:
```shell
dotnet add package Hexarc.Pact.Client
```
2. Install the `Hexarc.Pact.Tool` tool in the project:
```shell
dotnet tool install Hexarc.Pact.Tool
```
You may need to setup a .NET tools manifesto before 
install the `Hexarc.Pact.Tool` tool:
```shell
dotnet new tool-manifest
```
3. Add a `pact.json` config in the project with a desired API client 
   generation settings:
```json
{
   "schemaUri": "YOUR_API_HOST/pact/schema",
   "clientClassName": "DemoClient",
   "clientClassNamespace": "DemoNamespace",
   "outputDirectory": "Generated"
}
```
where
* `schemaUri` - link to a Pact API schema
* `clientClassName` - name for a generated API client class
* `clientClassNamespace` - namespace where to put in the generated API client class
* `outputDirectory` - output directory for the generated sources
4. Generate the API client via the Pact CLI tool:
```shell
dotnet pact
```
This command must be performed in the same folder with the `pact.json` config.

5. The generated API client will be available for accessing the API server:
```c#
var client = new DemoClient(new HttpClient { BaseAddress = new Uri("YOUR_API_HOST") });
var pong = await client.Misc.Ping("World");
Console.WriteLine(pong); // Prints "Hello, World" to the output.
```

## License
MIT © [Max Koverdyaev](https://github.com/shadeglare)
