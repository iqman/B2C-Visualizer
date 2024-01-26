using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class Secret
    {
        [JsonPropertyName("keyId")]
        public required string Id { get; set; }
        [JsonPropertyName("displayName")]
        public required string DisplayName { get; set; }
        [JsonPropertyName("hint")]
        public required string Hint { get; set; }
        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }
    }
}