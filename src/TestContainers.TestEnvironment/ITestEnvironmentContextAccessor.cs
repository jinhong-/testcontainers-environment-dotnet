namespace TestContainers.TestEnvironment
{
    public interface ITestEnvironmentContextAccessor
    {
        TestEnvironmentContext Context { get; set; }
    }
}