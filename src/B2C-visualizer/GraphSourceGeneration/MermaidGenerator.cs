using B2C_visualizer.Model;

namespace B2C_visualizer.GraphSourceGeneration
{
    internal class MermaidGenerator : IGraphGenerator
    {
        public string Generate(IEnumerable<ServicePrincipal> sps)
        {
            // , ""defaultRenderer"": ""elk""
            // ""htmlLabels"": false,

            var header =
@"%%{ init: { ""flowchart"": { 'nodeSpacing': 15, 'rankSpacing': 250, ""defaultRenderer"": ""elk""} } }%%
graph LR";

            string mermaid = string.Join(Environment.NewLine, header, string.Join(Environment.NewLine, sps.Select(GenerateForServicePrincipal)));

            return mermaid;
        }

        private string GenerateForServicePrincipal(ServicePrincipal sp)
        {
            var subgraph = GenerateSubGraph(sp);
            var relations = GenerateRelations(sp);

            return string.Join(Environment.NewLine, subgraph, relations);
        }

        private string GenerateRelations(ServicePrincipal sp)
        {

            // The "Where(r => r.AppId != sp.AppId)" filters away references from a given sp to app/oauth2 roles it has defined itself
            // Such a scenario is valid, but the default mermaid renderer crashes when trying to render this (bugs have been reported)

            var relations = sp.GrantedResourceAccesses.Any()
                ? string.Join(Environment.NewLine, sp.GrantedResourceAccesses.Where(r => r.AppId != sp.AppId).SelectMany(res => res.Roles).Select(r => $"{sp.AppId.ShortenId()} --> {r.Id.ShortenId()}"))
                : string.Empty;

            return relations;
        }

        private string GenerateSubGraph(ServicePrincipal sp)
        {
            string appRoles = GenerateAppRoleNodes(sp);

            string oauth2Permissions = GenerateOauth2RoleNodes(sp);

            var subgraph = $"""
subgraph {sp.AppId.ShortenId()}["{sp.Name}"]
  {sp.AppId.ShortenId()}_link["<u>B2C</u>"]
  style {sp.AppId.ShortenId()}_link fill:#fff0,stroke:#fff0

  subgraph {sp.AppId.ShortenId()}_app_roles["App Roles"]
{appRoles}
  end
  subgraph {sp.AppId.ShortenId()}_scopes["Scopes"]
{oauth2Permissions}
  end
end

click {sp.AppId.ShortenId()}_link href "https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Overview/appId/{sp.AppId}/isMSAApp~/false"

""";

            return subgraph;
        }

        private static string GenerateOauth2RoleNodes(ServicePrincipal sp)
        {
            return sp.DefinedOauth2Permissions.Any()
                ? string.Join(Environment.NewLine, sp.DefinedOauth2Permissions.Select(r => $"    {r.Id.ShortenId()}[\"{r.Value}\"]"))
                : $"    {sp.AppId.ShortenId()}_oauth2_perm_none[\"`*None*`\"]";
        }

        private static string GenerateAppRoleNodes(ServicePrincipal sp)
        {
            return sp.DefinedAppRoles.Any()
                ? string.Join(Environment.NewLine, sp.DefinedAppRoles.Select(r => $"    {r.Id.ShortenId()}[\"{r.Value}\"]"))
                : $"    {sp.AppId.ShortenId()}_app_roles_none[\"`*None*`\"]";
        }
    }
}
