using System;
using Microsoft.Extensions.Configuration;
using Samhammer.Options.Abstractions;

namespace Samhammer.Options
{
    internal static class ConfigurationResolver
    {
        internal static IConfiguration GetSection(IConfiguration configuration, Type optionType, OptionAttribute optionAttribute)
        {
            var sectionName = GetSectionName(optionType, optionAttribute);
            return optionAttribute.FromRootSection ? configuration : configuration.GetSection(sectionName);
        }

        internal static IConfiguration GetSection(IConfiguration configuration, Type optionType)
        {
            var sectionName = GetSectionName(optionType);
            return configuration.GetSection(sectionName);
        }

        private static string GetSectionName(Type optionType, OptionAttribute attribute)
        {
            var key = attribute.SectionName;

            if (string.IsNullOrWhiteSpace(key))
            {
                key = optionType.Name;
            }

            return key;
        }

        private static string GetSectionName(Type optionType)
        {
            return optionType.Name;
        }
    }
}
