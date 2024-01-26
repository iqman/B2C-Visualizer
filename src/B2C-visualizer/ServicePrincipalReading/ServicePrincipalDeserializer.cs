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
                if (sp != null) sps.Add(sp);
            }

            return sps;
        }
    }

    [JsonSerializable(typeof(ServicePrincipal))]
    internal partial class ServicePrincipalSourceGenerationContext : JsonSerializerContext
    {
    }
}
