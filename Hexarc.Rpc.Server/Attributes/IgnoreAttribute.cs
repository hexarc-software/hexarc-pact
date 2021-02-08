using System;

namespace Hexarc.Rpc.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class IgnoreAttribute : Attribute { }
}
