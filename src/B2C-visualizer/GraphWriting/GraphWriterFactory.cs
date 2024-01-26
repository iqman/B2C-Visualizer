namespace B2C_visualizer.GraphWriting
{
    class GraphWriterFactory
    {
        public IGraphWriter CreateGraphWriter(CommandlineOptions opts)
        {
            if (string.IsNullOrWhiteSpace(opts.OutputPaths))
            {
                return new StdOutGraphWriter();
            }
            else
            {
                return new FileGraphWriter(opts.OutputPaths);
            }
        }
    }
}
