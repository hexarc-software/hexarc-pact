using System;
using System.Collections.Generic;
using NUnit.Framework;
using Hexarc.Annotations;
using Hexarc.Rpc.Protocol.TypeProviders;
using Hexarc.Rpc.Server.Internals;
using Hexarc.Rpc.Server.Readers;

namespace Hexarc.Rpc.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void ReadTypes()
        {
            var typeChecker = new TypeChecker(
                new PrimitiveTypeProvider(),
                new ArrayLikeTypeProvider(),
                new DictionaryTypeProvider(),
                new TaskTypeProvider());
            var distinctTypeQueue = new DistinctTypeQueue();
            var typeReferenceReader = new TypeReferenceReader(typeChecker, distinctTypeQueue);
            var distinctTypeReader = new DistinctTypeReader(typeChecker, typeReferenceReader);

            var types = new[]
            {
                // typeof(Registry<>),
                typeof(Hexarc.Rpc.Protocol.Types.Type)
            };

            foreach (var t in types)
            {
                Console.WriteLine(ObjectDumper.Dump(typeReferenceReader.Read(t)));
            }

            while (distinctTypeQueue.TryDequeue(out var t))
            {
                Console.WriteLine(ObjectDumper.Dump(distinctTypeReader.Read(t)));
            }
        }

        public struct Point
        {
            public Int32 X { get; set; }
            public Int32 Y { get; set; }
        }

        public class Triangle
        {
            public Point V1 { get; set; }
            public Point V2 { get; set; }
            public Point V3 { get; set; }
        }

        public enum Status
        {
            Open,
            Close,
            Unknown
        }

        public class Registry<T>
        {
            public Guid Id { get; set; }

            [NullableReference]
            public Triangle? Triangle { get; set; }

            [NullableReference]
            public Dictionary<String, T>? Items { get; set; }

            [NullableReference]
            public List<List<Int32>>? Indices { get; set; }

            public DateTime Created { get; set; }

            public Status Status { get; set; }
        }
    }
}
