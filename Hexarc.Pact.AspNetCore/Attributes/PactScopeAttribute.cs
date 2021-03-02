using System;

namespace Hexarc.Pact.AspNetCore.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PactScopeAttribute : Attribute
    {
        public String Scope { get; }

        public PactScopeAttribute(String scope) => this.Scope = scope;
    }
}
