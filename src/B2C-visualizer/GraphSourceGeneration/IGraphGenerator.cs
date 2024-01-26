using B2C_visualizer.Model;

namespace B2C_visualizer.GraphSourceGeneration
{
    internal interface IGraphGenerator
    {
        string Generate(IEnumerable<ServicePrincipal> sps);
    }
}
