using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace TestContainers.TestEnvironment
{
    public class GenericHostFixture<TConfigurator> : IAsyncLifetime
        where TConfigurator : IConfigureGenericHostFixture, new()
    {
        private readonly ILogger<GenericHostFixture<TConfigurator>> _logger;
        private IHost _host;

        public GenericHostFixture(ILogger<GenericHostFixture<TConfigurator>> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            var hostBuilder = Host.CreateDefaultBuilder();
            var configurator = new TConfigurator();
            configurator.Configure(hostBuilder);
            _host = await hostBuilder.StartAsync();
        }

        public async Task DisposeAsync()
        {
            if (_host == null) return;
            try
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occured while cleaning up resources");
            }
        }
    }
}