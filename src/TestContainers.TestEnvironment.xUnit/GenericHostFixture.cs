using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestContainers.TestEnvironment
{
    public class GenericHostFixture<TConfigurator> : IAsyncLifetime
        where TConfigurator : IConfigureGenericHostFixture, new()
    {
        private readonly IMessageSink _messageSink;
        private IHost _host;

        public GenericHostFixture(IMessageSink messageSink)
        {
            _messageSink = messageSink;
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
                _messageSink.OnMessage(new DiagnosticMessage("Error occured while cleaning up resources: {0}", ex));
            }
        }
    }
}