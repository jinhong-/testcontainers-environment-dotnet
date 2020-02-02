using Xunit;

namespace TestContainers.TestEnvironment
{
    [Collection("TestEnvironment")]
    public class TestEnvironmentTests
    {
        [Fact]
        public void TestEnvironmentSetupAndTearDown()
        {
            //Refer to TestEnvironmentFixtureCollection.cs and TestEnvironmentConfigurator.cs
        }
    }
}