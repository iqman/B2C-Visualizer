using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class Resource
    {
        [JsonPropertyName("resourceAppId")]
        public required string AppId { get; set; }

        [JsonPropertyName("resourceAccess")]
        public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
    }
}