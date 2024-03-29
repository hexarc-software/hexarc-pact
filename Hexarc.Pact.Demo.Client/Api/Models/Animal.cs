// <auto-generated>
//   This code was generated by the Hexarc Pact tool. Do not edit.
// </auto-generated>

#nullable enable

namespace Hexarc.Pact.Demo.Api.Models
{
    [Hexarc.Serialization.Union.UnionTagAttribute(nameof(Kind))]
    [Hexarc.Serialization.Union.UnionCaseAttribute(typeof(Cat), "Cat")]
    [Hexarc.Serialization.Union.UnionCaseAttribute(typeof(Dog), "Dog")]
    public abstract class Animal
    {
        public abstract System.String Kind { get; }
    }

    public sealed class Cat : Animal
    {
        public override System.String Kind { get; } = "Cat";
        public System.Boolean IsMeowing { get; set; }
    }

    public sealed class Dog : Animal
    {
        public override System.String Kind { get; } = "Dog";
        public System.Boolean IsBarking { get; set; }
    }
}

#nullable restore