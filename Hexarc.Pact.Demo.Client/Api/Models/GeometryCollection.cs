// <auto-generated>
//   This code was generated by the Hexarc Pact tool. Do not edit.
//   Created: 2021-02-26 21:52:36Z
// </auto-generated>

#nullable enable

namespace Hexarc.Pact.Demo.Api.Models
{
    public sealed class GeometryCollection
    {
        public System.Collections.Generic.List<Point> Points { get; set; } = default!;
        public System.Collections.Generic.List<Hexarc.Pact.Demo.Api.ModelsV2.Point>? PointsV2 { get; set; }
        public System.Collections.Generic.Dictionary<System.Guid, Point> PointsDict { get; set; } = default!;
        public System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int32, System.String?>?> Boxes { get; set; } = default!;
        public Info<System.String, System.String?>? Info { get; set; }
        public Direction Direction { get; set; }
        public Flags Flags { get; set; }
    }
}

#nullable restore