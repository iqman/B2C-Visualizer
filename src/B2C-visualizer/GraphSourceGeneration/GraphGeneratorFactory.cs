
namespace B2C_visualizer.GraphSourceGeneration
{
    internal class GraphGeneratorFactory
    {
        public IGraphGenerator CreateGenerator(GraphFormat format)
        {
            switch (format)
            {
                case GraphFormat.Mermaid:
                    return new MermaidGenerator();
                case GraphFormat.PlantUML:
                    return new PlantUmlGenerator();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
