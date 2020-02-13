using Microsoft.Extensions.DependencyInjection;

namespace Samhammer.Options
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ResolveOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<OptionsResolver>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var optionsResolver = serviceProvider.GetRequiredService<OptionsResolver>();

            optionsResolver.ResolveConfigurations(serviceCollection);
            return serviceCollection;
        }
    }
}
