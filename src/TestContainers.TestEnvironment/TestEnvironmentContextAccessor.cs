namespace TestContainers.TestEnvironment
{
    public class TestEnvironmentContextAccessor : ITestEnvironmentContextAccessor
    {
        public TestEnvironmentContext Context { get; set; }
    }
}