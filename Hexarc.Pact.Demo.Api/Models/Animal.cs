using System;
using Hexarc.Annotations;

namespace Hexarc.Pact.Demo.Api.Models
{
    [UnionTag(nameof(Kind))]
    [UnionCase(typeof(Cat), AnimalKind.Cat)]
    [UnionCase(typeof(Dog), AnimalKind.Dog)]
    public abstract class Animal
    {
        public abstract String Kind { get; }
    }

    public sealed class Cat : Animal
    {
        public override String Kind { get; } = AnimalKind.Cat;

        public Boolean IsMeowing { get; set; }
    }

    public sealed class Dog : Animal
    {
        public override String Kind { get; } = AnimalKind.Dog;

        public Boolean IsBarking { get; set; }
    }

    public static class AnimalKind
    {
        public const String Cat = nameof(Cat);

        public const String Dog = nameof(Dog);
    }
}
