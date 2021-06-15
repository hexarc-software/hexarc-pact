using System;

namespace Hexarc.Pact.AspNetCore.Attributes
{
    /// <summary>
    /// Allows to put a Web API controller class to a specific scope
    /// which can be used as a filter during API schema fetching.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PactScopeAttribute : Attribute
    {
        public String Scope { get; }

        public PactScopeAttribute(String scope) => this.Scope = scope;
    }
}
