
namespace B2C_visualizer.ServicePrincipalReading
{
    internal interface IServicePrincipalReader
    {
        IEnumerable<string> GetServicePrincipalPresentationList();

        IEnumerable<string> GetServicePrincipals();
    }
}
