using B2C_visualizer.GraphSourceGeneration;
using B2C_visualizer.GraphWriting;
using B2C_visualizer.ServicePrincipalReading;
using CommandLine;
using System.Diagnostics.CodeAnalysis;


internal class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CommandlineOptions))]
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandlineOptions>(args).WithParsed(Run);

        static void Run(CommandlineOptions opts)
        {
            try
            {
                ServicePrincipalReaderFactory fac = new ServicePrincipalReaderFactory();
                var reader = fac.GetServicePrincipalReader(opts);


                if (opts.AskConfirmation)
                {
                    var presentationList = reader.GetServicePrincipalPresentationList();

                    Console.WriteLine($"The list of files contains {presentationList.Count()} entries and the complete list of files is");
                    foreach (var entry in presentationList)
                    {
                        Console.WriteLine($" - {entry}");
                    }

                    Console.WriteLine("Press y to continue, another other key to abort");
                    var key = Console.ReadKey(true);
                    if (key.KeyChar != 'y')
                    {
                        Console.WriteLine($"Received {key.KeyChar}. Aborting.");
                        return;
                    }
                }

                Console.Write("Reading service principals...");
                var stringifiedServicePrincipals = reader.GetServicePrincipals();
                Console.WriteLine("Done");


                Console.Write("Deserializing service principals...");
                ServicePrincipalDeserializer deserializer = new ServicePrincipalDeserializer();
                var sps = deserializer.Deserialize(stringifiedServicePrincipals);
                Console.WriteLine("Done");

                Console.Write("Generating output...");
                var generatorFactory = new GraphGeneratorFactory();
                var generator = generatorFactory.CreateGenerator(opts.OutputFormat);

                var output = generator.Generate(sps);
                Console.WriteLine("Done");

                Console.WriteLine("Writing output...");
                var graphWriterFactory = new GraphWriterFactory();
                var graphWriter = graphWriterFactory.CreateGraphWriter(opts);
                graphWriter.WriteGraph(output);

                Console.WriteLine("Writing output completed successfully. Press any key to close.");
                _ = Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Encountered an exception : {e}");
            }
        }
    }
}