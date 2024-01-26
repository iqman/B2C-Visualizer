namespace B2C_visualizer.GraphSourceGeneration
{
    static class StringExtensions
    {
        public static string ShortenId(this string id)
        {
            return "a" + id[..8];
        }
    }
}
