using System;

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
            (this.V1.X * (this.V2.Y - this.V3.Y) +
             this.V2.X * (this.V3.Y - this.V1.Y) +
             this.V3.X * (this.V1.Y - this.V2.Y)) / 2.0;
    }
}
