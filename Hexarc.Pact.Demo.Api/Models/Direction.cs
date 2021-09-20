namespace Hexarc.Pact.Demo.Api.Models;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
