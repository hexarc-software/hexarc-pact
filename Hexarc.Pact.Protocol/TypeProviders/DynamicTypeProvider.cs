using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    public sealed class DynamicTypeProvider
    {
        public readonly DynamicType Object = new(typeof(Object));

        public readonly DynamicType JsonElement = new(typeof(JsonElement));

        public IReadOnlySet<Guid> TypeIds { get; }

        public DynamicTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<DynamicType> Enumerate()
        {
            yield return this.Object;
            yield return this.JsonElement;
        }
    }
}
