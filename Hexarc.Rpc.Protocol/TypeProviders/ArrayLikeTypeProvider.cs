using System;
using System.Collections.Generic;
using System.Linq;
using Hexarc.Rpc.Protocol.Types;

namespace Hexarc.Rpc.Protocol.TypeProviders
{
    public sealed class ArrayLikeTypeProvider
    {
        public readonly ArrayLikeType EnumerableOfT = new(typeof(IEnumerable<>));

        public readonly ArrayLikeType ListOfT = new(typeof(List<>));

        public readonly ArrayLikeType HashSetOf = new(typeof(HashSet<>));

        public IReadOnlySet<Guid> TypeIds { get; }

        public ArrayLikeTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<ArrayLikeType> Enumerate()
        {
            yield return this.EnumerableOfT;
            yield return this.ListOfT;
            yield return this.HashSetOf;
        }
    }
}
