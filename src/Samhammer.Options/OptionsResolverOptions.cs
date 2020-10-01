using Microsoft.Extensions.Logging;
using Samhammer.Options.Strategy;

namespace Samhammer.Options
{
    public class OptionsResolverOptions
    {
        public ILoggerFactory LoggerFactory { get; set; }

        public IAssemblyResolvingStrategy AssemblyResolvingStrategy { get; set; }
    }
}
