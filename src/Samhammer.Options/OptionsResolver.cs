using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Samhammer.Options.Abstractions;
using Samhammer.Options.Utils;

namespace Samhammer.Options
{
    public class OptionsResolver
    {
        private IConfiguration Configuration { get; }

        private ILogger<OptionsResolver> Logger { get; }

        private MethodInfo AddOptionMethod { get; }

        public OptionsResolver(IConfiguration configuration, ILogger<OptionsResolver> logger)
        {
            Configuration = configuration;
            Logger = logger;
            AddOptionMethod = GetAddOptionMethod();
        }

        public void ResolveConfigurations(IServiceCollection services)
        {
            Logger.LogInformation("Start option initialization");

            var assemblies = AssemblyUtils.LoadAllAssembliesOfProject();
            Logger.LogTrace("Loaded assemblies: {Assemblies}.", assemblies.Select(a => a.GetName().Name));

            var types = ReflectionUtils.FindAllExportedTypesWithAttribute(assemblies, typeof(OptionAttribute));
            Logger.LogTrace("Loaded types with attribute {Attribute}: {Types}.", typeof(OptionAttribute), types);

            foreach (var type in types)
            {
                foreach (var attribute in ReflectionUtils.GetAttributesOfType<OptionAttribute>(type))
                {
                    ResolveConfiguration(services, type, attribute);
                }
            }

            Logger.LogInformation("Finished option initialization");
        }

        private void ResolveConfiguration(IServiceCollection services, Type optionType, OptionAttribute optionAttribute)
        {
            var sectionName = GetSectionName(optionType, optionAttribute);
            var section = optionAttribute.FromRootSection ? Configuration : Configuration.GetSection(sectionName);

            var genericMethod = AddOptionMethod.MakeGenericMethod(optionType);
            genericMethod.Invoke(this, new object[] { services, section, optionAttribute.IocName });
        }

        private string GetSectionName(Type optionType, OptionAttribute attribute)
        {
            var key = attribute.SectionName;

            if (string.IsNullOrWhiteSpace(key))
            {
                key = optionType.Name;
            }

            return key;
        }

        private void AddOption<T>(IServiceCollection services, IConfiguration section, string iocOptionName = null) where T : class
        {
            services.Configure<T>(
                iocOptionName ?? Microsoft.Extensions.Options.Options.DefaultName,
                section);

            Logger.LogDebug("Configured option {Option}", typeof(T));
        }

        private MethodInfo GetAddOptionMethod()
        {
            var method = typeof(OptionsResolver).GetMethod(nameof(AddOption), BindingFlags.Instance | BindingFlags.NonPublic);

            if (method == null)
            {
                throw new ApplicationException($"method {nameof(AddOption)} not found");
            }

            return method;
        }
    }
}
