using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock dictionary type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class DictionaryTypeProvider
    {
        private DictionaryType Dictionary { get; } = new(typeof(Dictionary<,>));

        private DictionaryType IDictionary { get; } = new(typeof(IDictionary<,>));

        private DictionaryType ReadOnlyDictionary { get; } = new(typeof(ReadOnlyDictionary<,>));

        private DictionaryType IReadOnlyDictionary { get; } = new(typeof(IReadOnlyDictionary<,>));

        /// <summary>
        /// Gets the registered dictionary type ids.
        /// </summary>
        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the DictionaryTypeProvider class.
        /// </summary>
        public DictionaryTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        /// <summary>
        /// Enumerates the registered dictionary types.
        /// </summary>
        /// <returns>The registered dictionary type collection.</returns>
        public IEnumerable<DictionaryType> Enumerate()
        {
            yield return this.Dictionary;
            yield return this.IDictionary;
            yield return this.ReadOnlyDictionary;
            yield return this.IReadOnlyDictionary;
        }
    }
}
