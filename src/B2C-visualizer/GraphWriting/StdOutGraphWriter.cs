namespace B2C_visualizer.GraphWriting
{
    class StdOutGraphWriter : IGraphWriter
    {
        public void WriteGraph(string graph)
        {
            Console.WriteLine("##################### Output begins #####################");
            Console.WriteLine(graph);
            Console.WriteLine("##################### Output ends #####################");
        }
    }
}
