namespace PollyResilienceApp.Configurations
{
    public class PollySettings
    {
        public CircuitBreakerSettings CircuitBreaker { get; set; }
        public RetrySettings Retry { get; set; }
        public TimeoutSettings Timeout { get; set; }
        public BulkheadSettings Bulkhead { get; set; }
    }

    public class CircuitBreakerSettings
    {
        public TimeSpan DurationOfBreak { get; set; }
        public int FailureThreshold { get; set; }
        public int MinimumThroughput { get; set; }
    }

    public class RetrySettings
    {
        public int Count { get; set; }
        public TimeSpan Delay { get; set; }
    }

    public class TimeoutSettings
    {
        public TimeSpan Duration { get; set; }
    }

    public class BulkheadSettings
    {
        public int MaxParallelization { get; set; }
        public int MaxQueuingActions { get; set; }
    }
}
