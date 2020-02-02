using Microsoft.Extensions.DependencyInjection;

namespace TestContainers.TestEnvironment
{
    public interface ITestContainersEnvironmentBuilder
    {
        IServiceCollection Services { get; }
    }
}