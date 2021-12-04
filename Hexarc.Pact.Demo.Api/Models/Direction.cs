using System.Text.Json.Serialization;

namespace Hexarc.Pact.Demo.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
