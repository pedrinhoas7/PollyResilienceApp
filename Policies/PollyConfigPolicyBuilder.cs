using Microsoft.Extensions.Options;
using Polly;
using PollyResilienceApp.Configurations;
using Microsoft.Extensions.Http;

namespace PollyResilienceApp.Policies
{
    public static class PollyConfigPolicyBuilder
    {
        public static IAsyncPolicy<HttpResponseMessage> Build(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<PollySettings>>().Value;

            // Política de Retry
            var retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(options.Retry.Count, _ => options.Retry.Delay);

            // Política de Circuit Breaker
            var circuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(options.CircuitBreaker.FailureThreshold, options.CircuitBreaker.DurationOfBreak);

            // Política de Timeout
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(options.Timeout.Duration);

            // Política de Bulkhead
           // var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(options.Bulkhead.MaxParallelization, options.Bulkhead.MaxQueuingActions);

            // Empacotando todas as políticas em uma única política combinada
            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
        }

        // Método para criar um handler de política usando o serviceProvider
        public static HttpMessageHandler BuildHandler(IServiceProvider serviceProvider)
        {
            // Chama a função Build para obter a política combinada
            var policy = Build(serviceProvider);

            // Retorna o PolicyHttpMessageHandler
            return new PolicyHttpMessageHandler(policy);
        }
    }
}
