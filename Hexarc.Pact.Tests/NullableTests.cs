using System;
using Namotion.Reflection;
using NUnit.Framework;

namespace Hexarc.Pact.Tests;

[TestFixture]
public class NullableTests
{
    [Test]
    public void StructCheck()
    {
        var t = typeof(Shape);
        Console.WriteLine(t.GetProperty(nameof(Shape.Point))!.ToContextualProperty().Nullability);
        Console.WriteLine(t.GetProperty(nameof(Shape.More))!.ToContextualProperty().Nullability);
        Console.WriteLine(t.GetProperty(nameof(Shape.X))!.ToContextualProperty().Nullability);
    }

    public struct Point
    {

    }

    public class More
    {

    }

    public class Shape
    {
        public Point Point { get; set; }

        public More? More { get; set; }

        public Int32? X { get; set; }
    }
}
