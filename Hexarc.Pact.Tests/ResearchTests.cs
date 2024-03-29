using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexarc.Pact.Client;
using NUnit.Framework;

namespace Hexarc.Pact.Tests;

[TestFixture]
public class ResearchTests
{
    [Test]
    public void GetAllTypeMembers()
    {
        Console.WriteLine(ObjectDumper.Dump(typeof(ControllerBase)
            .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(x => x.Name)));
    }

    [Test]
    public void StringEnum()
    {
        var m = new M { Box = Box.Off };
        var n = JsonSerializer.Deserialize<M>(JsonSerializer.Serialize(m));
        Console.WriteLine(ObjectDumper.Dump(n));
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    private enum Box
    {
        On,
        Off
    }

    private class M
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once PropertyCanBeMadeInitOnly.Local
        public Box Box { get; set; } = Box.On;
    }
}
