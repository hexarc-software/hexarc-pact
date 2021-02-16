namespace Hexarc.Pact.Tool.Extensions
{
    public static class ArrayFactory
    {
        public static T[] FromOne<T>(T element) => new[] { element };
    }
}
