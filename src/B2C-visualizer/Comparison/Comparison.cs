namespace B2C_visualizer.Comparison
{
    internal class Comparison
    {
        public required string Property { get; set; }
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public Similarity ExpectedSimilarity { get; set; }

        public Similarity ResultedSimilarity { get; set; }
        public string ResultedEnv1 { get; set; } = string.Empty;
        public string ResultedEnv2 { get; set; } = string.Empty;

        public int NestingLevel { get; set; }

        public bool IsParent { get; set; }
    }

    internal enum Similarity
    {
        Undefined,
        Same,
        Different,
        EnvSpecific,
        DontCare
    }
}