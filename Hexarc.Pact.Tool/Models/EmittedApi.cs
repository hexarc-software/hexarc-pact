using System.Collections.Generic;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class EmittedApi
    {
        public EmittedSource Client { get; }

        public IEnumerable<EmittedSource> Controllers { get; }

        public IEnumerable<EmittedSource> Models { get; }

        public EmittedApi(
            EmittedSource client,
            IEnumerable<EmittedSource> controllers,
            IEnumerable<EmittedSource> models)
        {
            this.Client = client;
            this.Controllers = controllers;
            this.Models = models;
        }
    }
}
