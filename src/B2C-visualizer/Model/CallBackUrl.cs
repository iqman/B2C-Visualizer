using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class CallBackUrl
    {
        [JsonPropertyName("url")]
        public required string Url { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
}