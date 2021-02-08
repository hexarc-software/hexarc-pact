using System;
using System.Collections.Generic;
using System.Linq;
using Hexarc.Rpc.Protocol.Types;

namespace Hexarc.Rpc.Protocol.TypeProviders
{
    public sealed class DictionaryTypeProvider
    {
        public readonly DictionaryType Dictionary = new(typeof(Dictionary<,>));

        public IReadOnlySet<Guid> TypeIds { get; }

        public DictionaryTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<DictionaryType> Enumerate()
        {
            yield return this.Dictionary;
        }
    }
}
