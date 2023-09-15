// <copyright file="Startup.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DefenderFileScanNotifier.Function.Startup))]

namespace DefenderFileScanNotifier.Function
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Azure.Identity;

    using DefenderFileScanNotifier.Function.Core.Logger;
    using DefenderFileScanNotifier.Function.Core.Services;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Guard=DefenderFileScanNotifier.Function.Core.ValidationHelper.GuardHelper;
    using DefenderFileScanNotifier.Function.Core.CoreConstants;

    /// <summary>
    /// The class represents the startup setup of the function app.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; private set; }

        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            _ = builder.Services.AddAzureAppConfiguration();
            this.ConfigureTelemetry(builder.Services);
            builder.Services.AddCoreDependencies(Configuration);
            this.ConfigureMalwareScanService(builder.Services);
        }

        /// <summary>
        /// Method to Configure Telemetry Services.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        private void ConfigureTelemetry(IServiceCollection services)
        {
            var telemetryConnectionString = this.Configuration[StartupConstants.InstrumentationConnectionStringConstant];
            _ = services.AddSingleton<IApplicationInsightsLogger>(_ => new ApplicationInsightsLogger(telemetryConnectionString));//.AILogger(loggingConfiguration, telemetryconfig, telemetryInitializers));
        }

     

        /// <inheritdoc/>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            Guard.ThrowIfInvalid(nameof(builder), builder);
            string executionEnv = Environment.GetEnvironmentVariable(StartupConstants.AzureFunctionsEnvironment);
            IConfigurationRoot configurationRoot = builder.ConfigurationBuilder.Build();

            _ = builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                _ = options.Connect(new Uri(configurationRoot[StartupConstants.AppConfigurationBaseUrl]), new DefaultAzureCredential())
                .Select($"{StartupConstants.CommonPrefix}*", executionEnv)
                .Select($"{StartupConstants.MalwareScannerPrefix}*", executionEnv)
                .TrimKeyPrefix(StartupConstants.CommonPrefix)
                .TrimKeyPrefix(StartupConstants.MalwareScannerPrefix)
                .ConfigureKeyVault(keyVaultOptions =>
                {
                    _ = keyVaultOptions.SetCredential(new DefaultAzureCredential());
                })
                .ConfigureRefresh(refreshOptions =>
                {
                    _ = refreshOptions.Register($"{StartupConstants.CommonPrefix}*", label: executionEnv, refreshAll: true)
                    .Register($"{StartupConstants.MalwareScannerPrefix}*", label: executionEnv, refreshAll: true)
                    .SetCacheExpiration(TimeSpan.FromMinutes(Convert.ToDouble(configurationRoot[StartupConstants.CacheExpirationTimeInMinutes])));
                });
            });

            this.Configuration = builder.ConfigurationBuilder.Build();
        }

        /// <summary>
        /// Method to configure blob service.
        /// </summary>
        /// <param name="services">Service Collection</param>
        private void ConfigureMalwareScanService(IServiceCollection services)
        {
            services.Configure<MalwareScannerConfigs>(this.Configuration.GetSection(nameof(MalwareScannerConfigs)));
            services.Configure<BlobConfigOptions>(this.Configuration.GetSection(nameof(BlobConfigOptions)));
        }
    }
}
