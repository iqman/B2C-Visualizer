
namespace B2C_visualizer.ServicePrincipalReading
{
    class ServicePrincipalOnlineReader : IServicePrincipalReader
    {
        private IEnumerable<Guid> servicePrincipalIds;
        private string token;

        public ServicePrincipalOnlineReader(IEnumerable<Guid> servicePrincipalIds, string token)
        {
            this.servicePrincipalIds = servicePrincipalIds;
            this.token = token;
        }

        public IEnumerable<string> GetServicePrincipals()
        {
            HttpClient c = new HttpClient();
            c.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var stringifiedServicePrincipals = new List<string>();

            foreach (var servicePrincipal in servicePrincipalIds)
            {
                var url = GetUrlForServicePrincipalId(servicePrincipal);
                try
                {
                    var sp = c.GetStringAsync(url).Result;
                    stringifiedServicePrincipals.Add(sp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error making GET request to {url}. Error: {ex}");
                }
            }

            return stringifiedServicePrincipals;
        }

        public IEnumerable<string> GetServicePrincipalPresentationList()
        {
            return servicePrincipalIds.Select(GetUrlForServicePrincipalId);
        }

        private string GetUrlForServicePrincipalId(Guid id)
        {
            return $"https://graph.windows.net/myorganization/applicationsByAppId/{id}?api-version=2.0";
        }
    }
}