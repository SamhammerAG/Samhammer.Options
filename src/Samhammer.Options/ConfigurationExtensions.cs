using System.Linq;
using Microsoft.Extensions.Configuration;
using Samhammer.Options.Abstractions;
using Samhammer.Options.Utils;

namespace Samhammer.Options
{
    public static class ConfigurationExtensions
    {
        public static T GetOptions<T>(this IConfiguration configuration) where T : new()
        {
            var optionType = typeof(T);
            var attributes = ReflectionUtils.GetAttributesOfType<OptionAttribute>(optionType).ToList();

            var section = attributes.Count > 0
                ? ConfigurationResolver.GetSection(configuration, optionType, attributes.First())
                : ConfigurationResolver.GetSection(configuration, optionType);

            var option = section.Get<T>();
            return option != null ? option : new T();
        }
    }
}
