using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.Api
{
    /// <summary>
    /// Describes the Hexarc Pact API schema that can by used for any Web API provided by a backend.
    /// </summary>
    public class Schema
    {
        /// <summary>
        /// Gets all controllers exposed by the schema instance.
        /// </summary>
        public Controller[] Controllers { get; }

        /// <summary>
        /// Gets all types that can be used in the controllers' methods.
        /// </summary>
        public Type[] Types { get; }

        /// <summary>
        /// Creates an instance of the Schema class.
        /// </summary>
        /// <param name="controllers">The controllers exposed by the schema.</param>
        /// <param name="types">The types that can be used in the controllers' methods.</param>
        public Schema(Controller[] controllers, Type[] types) =>
            (this.Controllers, this.Types) = (controllers, types);
    }
}
