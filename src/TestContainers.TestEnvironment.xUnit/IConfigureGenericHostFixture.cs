using Microsoft.Extensions.Hosting;

namespace TestContainers.TestEnvironment
{
    public interface IConfigureGenericHostFixture
    {
        void Configure(IHostBuilder hostBuilder);
    }
}