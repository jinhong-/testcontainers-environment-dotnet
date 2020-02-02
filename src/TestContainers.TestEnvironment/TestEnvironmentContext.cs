using Docker.DotNet;

namespace TestContainers.TestEnvironment
{
    public class TestEnvironmentContext
    {
        public IDockerClient DockerClient { get; set; }
    }
}