// <auto-generated>
//   This code was generated by the Hexarc Pact tool. Do not edit.
//   Created: 2021-02-25 06:50:27Z
// </auto-generated>

#nullable enable

namespace Hexarc.Pact.Demo.Api.Controllers
{
    public sealed partial class AnimalController : Hexarc.Pact.Client.ControllerBase
    {
        public AnimalController(Hexarc.Pact.Client.ClientBase client, System.String controllerPath = "/Animals") : base(client, controllerPath)
        {
        }

        public async System.Threading.Tasks.Task<Hexarc.Pact.Demo.Api.Models.Animal?> GetAnimal(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, System.String>>? headers = default)
        {
            return await this.GetJsonOrNull<Hexarc.Pact.Demo.Api.Models.Animal?>("/GetAnimal", System.Array.Empty<Hexarc.Pact.Client.GetMethodParameter>(), headers);
        }
    }
}

#nullable restore