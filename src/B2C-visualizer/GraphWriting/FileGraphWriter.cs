namespace B2C_visualizer.GraphWriting
{
    class FileGraphWriter : IGraphWriter
    {
        private string path;

        public FileGraphWriter(string path)
        {
            this.path = path;
        }

        public void WriteGraph(string graph)
        {
            File.WriteAllText(path, graph);
        }
    }
}
