using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestContainers.Container.Abstractions;
using TestContainers.Container.Abstractions.DockerClient;
using TestContainers.Container.Abstractions.Reaper;

namespace TestContainers.TestEnvironment
{
    public class TestContainersEnvironmentService : IHostedService
    {
        private IEnumerable<IContainer> _containers;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITestEnvironmentContextAccessor _testEnvironmentContextAccessor;
        private readonly DockerClientFactory _dockerClientFactory;

        public TestContainersEnvironmentService(IServiceProvider serviceProvider,
            ITestEnvironmentContextAccessor testEnvironmentContextAccessor,
            DockerClientFactory dockerClientFactory)
        {
            _serviceProvider = serviceProvider;
            _testEnvironmentContextAccessor = testEnvironmentContextAccessor;
            _dockerClientFactory = dockerClientFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var dockerClient = await _dockerClientFactory.Create(cancellationToken);
            _testEnvironmentContextAccessor.Context = new TestEnvironmentContext {DockerClient = dockerClient};

            _containers = _serviceProvider.GetServices<IContainer>();
            foreach (var container in _containers)
            {
                await container.StartAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_containers == null) return;
            foreach (var container in _containers)
            {
                await container.StopAsync(cancellationToken);
            }
        }
    }
}