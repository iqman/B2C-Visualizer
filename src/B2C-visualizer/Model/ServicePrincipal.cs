using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace B2C_visualizer.Model
{
    internal class ServicePrincipal
    {
        [JsonPropertyName("appId")]
        public required string AppId { get; set; }
        [JsonPropertyName("appRoles")]
        public IEnumerable<Role> DefinedAppRoles { get; set; } = Enumerable.Empty<Role>();

        [JsonPropertyName("oauth2Permissions")]
        public IEnumerable<Role> DefinedOauth2Permissions { get; set; } = Enumerable.Empty<Role>();

        [JsonPropertyName("identifierUris")]
        public IEnumerable<string> IdentifierUris { get; set; } = Enumerable.Empty<string>();

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("passwordCredentials")]
        public IEnumerable<Secret> Secrets { get; set; } = Enumerable.Empty<Secret>();

        [JsonPropertyName("replyUrlsWithType")]
        public IEnumerable<CallBackUrl> CallbackUrls { get; set; } = Enumerable.Empty<CallBackUrl>();

        [JsonPropertyName("requiredResourceAccess")]
        public IEnumerable<Resource> GrantedResourceAccesses { get; set; } = Enumerable.Empty<Resource>();


    }
}
