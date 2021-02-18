using System;
using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Api
{
    public sealed class MethodResult
    {
        /// <summary>
        /// Gets the task-based method result type.
        /// </summary>
        public TaskTypeReference Type { get; }

        /// <summary>
        /// Gets the indicator that the result value has a reference type.
        /// </summary>
        public Boolean IsReference { get; }

        public MethodResult(TaskTypeReference type, Boolean isReference) =>
            (this.Type, this.IsReference) = (type, isReference);
    }
}
