using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock dynamic type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class DynamicTypeProvider
    {
        private DynamicType Object { get; } = new(typeof(Object));

        private DynamicType JsonElement { get; } = new(typeof(JsonElement));

        /// <summary>
        /// Gets the registered dynamic type ids.
        /// </summary>
        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the DynamicTypeProvider class.
        /// </summary>
        public DynamicTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        /// <summary>
        /// Enumerates the registered dynamic types.
        /// </summary>
        /// <returns>The registered dynamic type collection.</returns>
        public IEnumerable<DynamicType> Enumerate()
        {
            yield return this.Object;
            yield return this.JsonElement;
        }
    }
}
