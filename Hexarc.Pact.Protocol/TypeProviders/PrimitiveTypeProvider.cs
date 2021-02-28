using System;
using System.Collections.Generic;
using System.Linq;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock primitive type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class PrimitiveTypeProvider
    {
        private PrimitiveType Boolean { get; } = new(typeof(Boolean));

        private PrimitiveType Byte { get; } = new(typeof(Byte));

        private PrimitiveType SByte { get; } = new(typeof(SByte));

        private PrimitiveType Char { get; } = new(typeof(Char));

        private PrimitiveType Int16 { get; } = new(typeof(Int16));

        private PrimitiveType UInt16 { get; } = new(typeof(UInt16));

        private PrimitiveType Int32 { get; } = new(typeof(Int32));

        private PrimitiveType UInt32 { get; } = new(typeof(UInt32));

        private PrimitiveType Int64 { get; } = new(typeof(Int64));

        private PrimitiveType UInt64 { get; } = new(typeof(UInt64));

        private PrimitiveType Single { get; } = new(typeof(Single));

        private PrimitiveType Double { get; }= new(typeof(Double));

        private PrimitiveType Decimal { get; } = new(typeof(Decimal));

        private PrimitiveType String { get; } = new(typeof(String));

        private PrimitiveType Guid { get; } = new(typeof(Guid));

        private PrimitiveType DateTime { get; } = new(typeof(DateTime));

        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the PrimitiveTypeProvider class.
        /// </summary>
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
