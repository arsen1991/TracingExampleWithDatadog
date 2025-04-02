using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tracing.Shared.TraceAndLong
{
    public static class ConfigureTracingAndLogging
    {
        private static TracingAndLoggingConfiguration configuration;

        public static void ConfigureTracing(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            BindConfiguration(configurationSection);

            services.AddTracing(configuration);
        }

        public static void ConfigureLogging(this ILoggingBuilder loggingBuilder, IConfigurationSection configurationSection)
        {
            BindConfiguration(configurationSection);

            loggingBuilder.AddLogging(configuration);
        }

        private static void BindConfiguration(IConfigurationSection configurationSection)
        {
            if (configuration == null)
            {
                configuration = new TracingAndLoggingConfiguration();

                configurationSection.Bind(configuration);
            }
        }
    }
}