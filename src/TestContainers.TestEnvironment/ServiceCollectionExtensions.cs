using Microsoft.Extensions.DependencyInjection;
using TestContainers.Container.Abstractions.DockerClient;

namespace TestContainers.TestEnvironment
{
    public static class ServiceCollectionExtensions
    {
        public static ITestContainersEnvironmentBuilder AddTestContainersEnvironment(this IServiceCollection services)
        {
            
            services.AddSingleton<ITestEnvironmentContextAccessor, TestEnvironmentContextAccessor>();
            services.AddSingleton<IDockerClientProvider, EnvironmentDockerClientProvider>();
            services.AddSingleton<IDockerClientProvider, NpipeDockerClientProvider>();
            services.AddSingleton<IDockerClientProvider, UnixDockerClientProvider>();
            services.AddSingleton<DockerClientFactory>();
            services.AddHostedService<TestContainersEnvironmentService>();
            return new TestContainersEnvironmentBuilder(services);
        }
    }
}