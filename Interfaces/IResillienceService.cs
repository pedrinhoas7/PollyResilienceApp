namespace PollyResilienceApp.Interfaces
{
    public interface IResillienceService
    {
        public Task<object> RetryAndCircuitBreak();
        public Task<object> ForceRetry();
        public Task<object> SendManyRequestsToEnableBulkhead();
    }
}
