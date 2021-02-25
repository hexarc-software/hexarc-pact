// <auto-generated>
//   This code was generated by the Hexarc Pact tool. Do not edit.
//   Created: 2021-02-25 06:50:27Z
// </auto-generated>

#nullable enable

namespace Hexarc.Pact.Demo.Api.Controllers
{
    public sealed partial class GeometryController : Hexarc.Pact.Client.ControllerBase
    {
        public GeometryController(Hexarc.Pact.Client.ClientBase client, System.String controllerPath = "/Geometry") : base(client, controllerPath)
        {
        }

        public async System.Threading.Tasks.Task<System.Double> Area(Hexarc.Pact.Demo.Api.Models.Triangle triangle, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, System.String>>? headers = default)
        {
            return await this.PostJson<Hexarc.Pact.Demo.Api.Models.Triangle, System.Double>("/Area", triangle, headers);
        }

        public async System.Threading.Tasks.Task<Hexarc.Pact.Demo.Api.Models.GeometryCollection?> ComputeCollection(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, System.String>>? headers = default)
        {
            return await this.GetJsonOrNull<Hexarc.Pact.Demo.Api.Models.GeometryCollection?>("/ComputeCollection", System.Array.Empty<Hexarc.Pact.Client.GetMethodParameter>(), headers);
        }
    }
}

#nullable restore