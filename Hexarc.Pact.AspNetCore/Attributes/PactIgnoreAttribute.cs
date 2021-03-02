using System;

namespace Hexarc.Pact.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class PactIgnoreAttribute : Attribute { }
}
