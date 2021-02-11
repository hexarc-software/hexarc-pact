using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Hexarc.Pact.AspNetCore.Internals
{
    public sealed class DistinctTypeQueue
    {
        private HashSet<Type> AllTypes { get; } = new();

        private Queue<Type> IncomeTypes { get; } = new();

        public void Enqueue(Type type)
        {
            if (this.AllTypes.Contains(type)) return;

            this.AllTypes.Add(type);
            this.IncomeTypes.Enqueue(type);
        }

        public Boolean TryDequeue([MaybeNullWhen(false)]out Type type) =>
            this.IncomeTypes.TryDequeue(out type);
    }
}
