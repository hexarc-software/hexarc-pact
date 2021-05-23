# Hexarc Pact
Hexarc Pact provides a toolchain for exposing and consuming Web API for .NET/TypeScript-based projects.

|Package|Platform|Version|Downloads|
|-------|--------|-------|---------|
|`Hexarch.Pact.Protocol`| .NET 5.0+ | [![NuGet](https://img.shields.io/nuget/v/Hexarc.Pact.Protocol.svg)](https://nuget.org/packages/Hexarc.Pact.Protocol) | [![Nuget](https://img.shields.io/nuget/dt/Hexarc.Pact.Protocol.svg)](https://nuget.org/packages/Hexarc.Pact.Protocol) |
|`Hexarch.Pact.AspNetCore`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.AspNetCore.svg)](https://nuget.org/packages/Hexarc.Pact.AspNetCore) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.AspNetCore.svg)](https://nuget.org/packages/Hexarc.Pact.AspNetCore) |
|`Hexarch.Pact.Client`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.Client.svg)](https://nuget.org/packages/Hexarc.Pact.Client) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.Client.svg)](https://nuget.org/packages/Hexarc.Pact.Client) |
|`Hexarch.Pact.Tool`| .NET 5.0+ | [![Version](http://img.shields.io/nuget/v/Hexarc.Pact.Tool.svg)](https://nuget.org/packages/Hexarc.Pact.Tool) | [![Downloads](https://img.shields.io/nuget/dt/Hexarc.Pact.Tool.svg)](https://nuget.org/packages/Hexarc.Pact.Tool) |
|`@hexarc/pact-tool`| TypeScript | [![Version](http://img.shields.io/npm/v/@hexarc/pact-tool.svg)](https://www.npmjs.org/package/@hexarc/pact-tool) | [![Downloads](http://img.shields.io/npm/dt/@hexarc/pact-tool.svg)](https://www.npmjs.org/package/@hexarc/pact-tool) |

## Getting started
* [Expose Pact API schema](#expose-pact-api-schema) (.NET Web API service)
* [Consume Pact API schema](#consume-pact-api-schema) (.NET client application)

## Features
* Hassle-free API integrations in seconds.
* No type information loosing. Consume exact API that is provided.
* Code first. No need to describe your API protocol in external IDL/DSL.
* Charged with advanced types. Not only simple DTOs but enums, generics, tagged union, etc.

## Type System
The Pact type system is based on the .NET CLR types with some bespoke ones
for the best development experience. 

What's inside the Pact type system:
* Primitive types (including `Guid` and `DateTime`)
* Value/Reference type semantics
* Collections and generics (not only simple arrays and dictionaries)
* Complete support for Nullable Reference Type annotations (NRT)
* Enums (string-base and number-based)
* Tagged Unions (via [Hexarc.Serialization.Union](https://github.com/hexarc-software/hexarc-serialization))
* Tuples (via [Hexarc.Serialization.Tuple](https://github.com/hexarc-software/hexarc-serialization))

## Expose Pact API schema
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
4. Implement an API controller according to the Pact conventions:
```c#
[ApiController, Route("Misc")]
public sealed class MiscController : ControllerBase
{
    [HttpGet, Route(nameof(Ping))]
    public String Ping([FromQuery] String message) => $"Hello, {message}";
}
```
5. Start the app and open the `YOUR_API_HOST/pact/schema` address to ensure 
   the Pact API schema is generated.
   
## Consume Pact API schema
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
* `clientClassName` - name for generated API client class
* `clientClassNamespace` - namespace where to put in the generated API client class
* `outputDirectory` - output directory for generated sources
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

