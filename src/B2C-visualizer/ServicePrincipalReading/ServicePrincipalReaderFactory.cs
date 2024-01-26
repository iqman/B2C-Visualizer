namespace B2C_visualizer.ServicePrincipalReading
{
    class ServicePrincipalReaderFactory
    {
        public IServicePrincipalReader GetServicePrincipalReader(CommandlineOptions opts)
        {
            if (opts.InputPaths is not null && opts.InputPaths.Any())
            {
                return new ServicePrincipalFileReader(opts.InputPaths);
            }

            if (opts.ServicePrincipalIds is not null && opts.ServicePrincipalIds.Any())
            {
                if (opts.Token is null) throw new ArgumentNullException(nameof(opts.Token));

                return new ServicePrincipalOnlineReader(opts.ServicePrincipalIds, opts.Token);
            }

            throw new NotImplementedException();
        }
    }
}