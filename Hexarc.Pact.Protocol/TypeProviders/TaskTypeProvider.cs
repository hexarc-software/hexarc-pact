using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock task type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class TaskTypeProvider
    {
        private TaskType TaskOfT { get; } = new(typeof(Task<>));

        private TaskType ValueTaskOfT { get; } = new(typeof(ValueTask<>));

        /// <summary>
        /// Gets the registered task type ids.
        /// </summary>
        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the TaskTypeProvider class.
        /// </summary>
        public TaskTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        /// <summary>
        /// Enumerates the registered task types.
        /// </summary>
        /// <returns>The registered task type collection.</returns>
        public IEnumerable<TaskType> Enumerate()
        {
            yield return this.TaskOfT;
            yield return this.ValueTaskOfT;
        }
    }
}
