using Microsoft.Extensions.DependencyInjection;

namespace TestContainers.TestEnvironment
{
    public class TestContainersEnvironmentBuilder : ITestContainersEnvironmentBuilder
    {
        public TestContainersEnvironmentBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}