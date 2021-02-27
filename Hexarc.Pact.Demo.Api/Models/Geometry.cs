using System;
using System.Collections.Generic;

namespace Hexarc.Pact.Demo.Api.Models
{
    public class Point
    {
        public Double X { get; set; }

        public Double Y { get; set; }
    }

    public class Triangle
    {
        public Point V1 { get; set; } = default!;

        public Point V2 { get; set; } = default!;

        public Point V3 { get; set; } = default!;

        public Double CalculateArea() =>
            Math.Abs(this.V1.X * (this.V2.Y - this.V3.Y) +
             this.V2.X * (this.V3.Y - this.V1.Y) +
             this.V3.X * (this.V1.Y - this.V2.Y)) / 2.0;
    }

    public class Info<T1, T2>
    {
        public List<T1?> Items { get; set; } = default!;

        public Dictionary<String, T2?> Marks { get; set; } = default!;
    }

    public class GeometryCollection
    {
        public List<Point> Points { get; set; } = default!;

        public List<ModelsV2.Point>? PointsV2 { get; set; } = default!;

        public Dictionary<Guid, Point> PointsDict { get; set; } = default!;

        public List<Dictionary<Int32, String?>?> Boxes { get; set; } = default!;

        public Info<String, String?>? Info { get; set; }

        public Direction Direction { get; set; }

        public Flags Flags { get; set; }
    }
}

namespace Hexarc.Pact.Demo.Api.ModelsV2
{
    public class Point
    {
        public Double X { get; set; }

        public Double Y { get; set; }
    }
}
