

namespace DefenderFileScanNotifier.Function.Core.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;


    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The <see cref="ServiceCollectionExtensions"/> class helps to create injected dependencies.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds core dependencies.
        /// </summary>
        /// <param name="services">The instance for <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The instance for <see cref="IConfiguration"/>.</param>
        public static void AddCoreDependencies(this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            _ = services.AddSingleton<IMalwareScannerManager, MalwareScannerManager>();
            _ = services.AddSingleton<IBlobClientRepository, BlobClientRepository>();
        }
    }
}
