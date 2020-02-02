using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestContainers.Container.Database.PostgreSql;

namespace TestContainers.TestEnvironment
{
    public class TestEnvironmentConfigurator : IConfigureGenericHostFixture
    {
        public void Configure(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging(b => b
                    .AddConsole()
                    .AddDebug())
                .ConfigureServices(services =>
                {
                    services.AddTestContainersEnvironment()
                        .RegisterContainer<PostgreSqlContainer>("postgres:11", c =>
                        {
                            c.Username = "postgres";
                        });
                });
        }
    }
}