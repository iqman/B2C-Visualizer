using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class Role
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; } = false;
        [JsonPropertyName("type")]

        [JsonConverter(typeof(JsonStringEnumConverter<RoleType>))]
        public RoleType Type { get; set; }
    }
}


