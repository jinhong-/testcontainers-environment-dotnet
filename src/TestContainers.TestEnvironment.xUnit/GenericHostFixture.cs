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

        public GenericHostFixture(IMessageSink messageSink)
        {
            _messageSink = messageSink;
        }

        public IHost Host { get; private set; }

        public async Task InitializeAsync()
        {
            var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
            var configurator = new TConfigurator();
            configurator.Configure(hostBuilder);
            Host = await hostBuilder.StartAsync();
        }

        public async Task DisposeAsync()
        {
            if (Host == null) return;
            try
            {
                await Host.StopAsync();
                Host.Dispose();
            }
            catch (Exception ex)
            {
                _messageSink.OnMessage(new DiagnosticMessage("Error occured while cleaning up resources: {0}", ex));
            }
        }
    }
}