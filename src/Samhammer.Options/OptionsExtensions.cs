using Microsoft.Extensions.Logging;
using Samhammer.Options.Strategy;

namespace Samhammer.Options
{
    public static class OptionsExtensions
    {
        public static OptionsResolverOptions SetLogging(this OptionsResolverOptions options, ILoggerFactory factory)
        {
            options.LoggerFactory = factory;
            return options;
        }

        public static OptionsResolverOptions SetStrategy(this OptionsResolverOptions options, IAssemblyResolvingStrategy strategy)
        {
            options.AssemblyResolvingStrategy = strategy;
            return options;
        }

        public static ILogger<T> BuildLogger<T>(this OptionsResolverOptions options)
        {
            return options.LoggerFactory.CreateLogger<T>();
        }
    }
}
