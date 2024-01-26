using B2C_visualizer.GraphSourceGeneration;
using CommandLine;

record CommandlineOptions
{
    [Option('s', "ids", SetName = "online", Required = true, HelpText = "Space separated list of IDs of Azure AD B2C Service principals to be processed. Must be GUIDs like e9877972-bb77-4c69-946d-f72b8d25b9d3")]
    public required IEnumerable<Guid> ServicePrincipalIds { get; set; }

    [Option('t', SetName = "online", Required = true, HelpText = "Jwt bearer token used to access the service principals. Required when using ids.")]
    public required string Token { get; set; }

    [Option('i', "input", SetName = "file", Required = true, HelpText = "Space separated list of paths to file with the raw JSON service principal definition. If pointing to a folder(s), then all *.json files in those folder(s) are read.")]
    public required IEnumerable<string> InputPaths { get; set; }

    [Option('o', Required = false, HelpText = "Filename to output to. Will output to stdout if omitted.")]
    public required string OutputPaths { get; set; }

    [Option('f', Required = false, Default = GraphFormat.Mermaid, HelpText = "Format of the output.")]
    public required GraphFormat OutputFormat { get; set; }

    [Option('c', Required = false, Default = true, HelpText = "Whether to print the service principals/files that will be processed and ask for confirmation to proceed.")]
    public required bool AskConfirmation { get; set; }

}
