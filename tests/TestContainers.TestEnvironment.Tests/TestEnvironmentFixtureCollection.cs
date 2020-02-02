using Xunit;

namespace TestContainers.TestEnvironment
{
    [CollectionDefinition("TestEnvironment")]
    public class TestEnvironmentFixtureCollection : ICollectionFixture<GenericHostFixture<TestEnvironmentConfigurator>>
    {
    }
}