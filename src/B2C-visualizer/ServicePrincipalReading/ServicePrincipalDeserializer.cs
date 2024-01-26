using B2C_visualizer.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace B2C_visualizer.ServicePrincipalReading
{
    internal interface IServicePrincipalDeserializer
    {
        IEnumerable<ServicePrincipal> Deserialize(IEnumerable<string> stringifiedServicePrincipals);
    }

    class ServicePrincipalDeserializer : IServicePrincipalDeserializer
    {
        public IEnumerable<ServicePrincipal> Deserialize(IEnumerable<string> stringifiedServicePrincipals)
        {
            IList<ServicePrincipal> sps = new List<ServicePrincipal>();

            foreach (var c in stringifiedServicePrincipals)
            {
                var sp = JsonSerializer.Deserialize(c, ServicePrincipalSourceGenerationContext.Default.ServicePrincipal);
                if (sp != null)
                {
                    AddDefaultScope(sp);
                    sps.Add(sp);
                }
            }

            return sps;
        }

        private void AddDefaultScope(ServicePrincipal sp)
        {
            if (!sp.DefinedOauth2Permissions.Any())
            {
                sp.DefinedOauth2Permissions = new List<Role>();
            }
            var scopes = new List<Role>
            {
                new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = "Default scope",
                    DisplayName = "Default scope",
                    IsEnabled = true,
                    Type = RoleType.Scope,
                    Value = ".default"
                }
            };
            sp.DefinedOauth2Permissions = scopes;
        }
    }

    [JsonSerializable(typeof(ServicePrincipal))]
    internal partial class ServicePrincipalSourceGenerationContext : JsonSerializerContext
    {
    }
}
