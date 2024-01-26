
namespace B2C_visualizer.ServicePrincipalReading
{
    class ServicePrincipalFileReader : IServicePrincipalReader
    {
        private readonly IEnumerable<string> filePaths;

        public ServicePrincipalFileReader(IEnumerable<string> filePaths)
        {
            this.filePaths = filePaths;
        }

        public IEnumerable<string> GetServicePrincipals()
        {
            var fileList = BuildFileList();

            var stringifiedServicePrincipals = new List<string>();

            foreach (var path in fileList)
            {
                var sp = File.ReadAllText(path);
                stringifiedServicePrincipals.Add(sp);
            }

            return stringifiedServicePrincipals;
        }

        private IEnumerable<string> BuildFileList()
        {
            var finalPaths = new List<string>();
            foreach (var path in filePaths)
            {
                var fullPath = Path.GetFullPath(path);

                if (File.Exists(fullPath))
                {
                    finalPaths.Add(fullPath);
                }
                else if (Directory.Exists(fullPath))
                {
                    finalPaths.AddRange(Directory.EnumerateFiles(fullPath, "*.json"));
                }
            }

            return finalPaths;
        }

        public IEnumerable<string> GetServicePrincipalPresentationList()
        {
            return BuildFileList();
        }
    }

}