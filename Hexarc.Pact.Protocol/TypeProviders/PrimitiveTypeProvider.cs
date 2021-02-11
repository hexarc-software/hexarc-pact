using System;
using System.Collections.Generic;
using System.Linq;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    public sealed class PrimitiveTypeProvider
    {
        public readonly PrimitiveType Boolean = new(typeof(Boolean));

        public readonly PrimitiveType Byte = new(typeof(Byte));

        public readonly PrimitiveType SByte = new(typeof(SByte));

        public readonly PrimitiveType Char = new(typeof(Char));

        public readonly PrimitiveType Int16 = new(typeof(Int16));

        public readonly PrimitiveType UInt16 = new(typeof(UInt16));

        public readonly PrimitiveType Int32 = new(typeof(Int32));

        public readonly PrimitiveType UInt32 = new(typeof(UInt32));

        public readonly PrimitiveType Int64 = new(typeof(Int64));

        public readonly PrimitiveType UInt64 = new(typeof(UInt64));

        public readonly PrimitiveType Single = new(typeof(Single));

        public readonly PrimitiveType Double = new(typeof(Double));

        public readonly PrimitiveType Decimal = new(typeof(Decimal));

        public readonly PrimitiveType String = new(typeof(String));

        public readonly PrimitiveType Guid = new(typeof(Guid));

        public readonly PrimitiveType DateTime = new(typeof(DateTime));

        public IReadOnlySet<Guid> TypeIds { get; }

        public PrimitiveTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<PrimitiveType> Enumerate()
        {
            yield return this.Boolean;
            yield return this.Byte;
            yield return this.SByte;
            yield return this.Char;
            yield return this.Int16;
            yield return this.UInt16;
            yield return this.Int32;
            yield return this.UInt32;
            yield return this.Int64;
            yield return this.UInt64;
            yield return this.Single;
            yield return this.Double;
            yield return this.Decimal;
            yield return this.String;
            yield return this.Guid;
            yield return this.DateTime;
        }
    }
}
