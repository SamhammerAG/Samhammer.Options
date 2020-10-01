using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Samhammer.Options.Strategy;

namespace Samhammer.Options
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ResolveOptions(this IServiceCollection serviceCollection, IConfiguration configuration, Action<OptionsResolverOptions> customizeOptions = null)
        {
            var options = BuildDefaultOptions();
            customizeOptions?.Invoke(options);

            var logger = options.BuildLogger<OptionsResolver>();
            var resolver = new OptionsResolver(configuration, options.AssemblyResolvingStrategy, logger);

            resolver.ResolveConfigurations(serviceCollection);
            return serviceCollection;
        }

        private static OptionsResolverOptions BuildDefaultOptions()
        {
            var options = new OptionsResolverOptions
            {
                LoggerFactory = new NullLoggerFactory(),
                AssemblyResolvingStrategy = new DefaultAssemblyResolvingStrategy(),
            };

            return options;
        }
    }
}
