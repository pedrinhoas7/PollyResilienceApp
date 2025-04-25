using Microsoft.Extensions.Options;
using Polly;
using PollyResilienceApp.Configurations;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PollyResilienceApp.Policies
{
    public static class PollyConfigPolicyBuilder
    {

        public static IAsyncPolicy<HttpResponseMessage> Build(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<PollySettings>>().Value;
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("PollyPolicy");


            // Política de Retry
            var retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(options.Retry.Count, _ => options.Retry.Delay, onRetry: (outcome, delay, retryCount, context) =>
                {
                    logger.LogWarning("Retry {RetryCount} after {Delay}. StatusCode: {StatusCode}",
                        retryCount, delay, outcome.Result?.StatusCode);
                }); ;

            // Política de Circuit Breaker
            var circuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(options.CircuitBreaker.FailureThreshold, options.CircuitBreaker.DurationOfBreak, onBreak: (outcome, breakDelay) =>
                {
                    logger.LogError("Circuit broken! StatusCode: {StatusCode}. Breaking for {BreakDelay}.",
                        outcome.Result?.StatusCode, breakDelay);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit reset. Back to normal.");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit is half-open. Next call is a trial.");
                });

            // Política de Timeout
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(options.Timeout.Duration);

            // Política de Bulkhead
            var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(options.Bulkhead.MaxParallelization, options.Bulkhead.MaxQueuingActions, onBulkheadRejectedAsync: context =>
            {
                logger.LogWarning("Bulkhead rejection occurred.");
                return Task.CompletedTask;
            });



            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy, bulkheadPolicy);
        }

        public static HttpMessageHandler BuildHandler(IServiceProvider serviceProvider)
        {
            var policy = Build(serviceProvider);
            return new PolicyHttpMessageHandler(policy);
        }
    }
}
