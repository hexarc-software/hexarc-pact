using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    public sealed class TaskTypeProvider
    {
        public readonly TaskType TaskOfT = new(typeof(Task<>));

        public readonly TaskType ValueTaskOfT = new(typeof(ValueTask<>));

        public IReadOnlySet<Guid> TypeIds { get; }

        public TaskTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<TaskType> Enumerate()
        {
            yield return this.TaskOfT;
            yield return this.ValueTaskOfT;
        }
    }
}
