using Polly;
using Polly.Bulkhead;
using Polly.Timeout;
using PollyResilienceApp.Configurations;
using PollyResilienceApp.Interfaces;

namespace PollyResilienceApp.Services
{
    public class ResillienceService : IResillienceService
    {
        private readonly IHttpClient _client;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ResillienceService> _logger;
        public ResillienceService(IHttpClient client, ILogger<ResillienceService> logger, HttpClient httpClient) 
        {
            _client = client;
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<object> RetryAndCircuitBreak()
        {
            try
            {
                return await _client.GetInternalServeError();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> ForceRetry()
        {
            var log = new List<string>();

            // TODO: Expor os logs via API para demonstrar claramente como a política de Retry funciona.
            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(5),
                    onRetry: (outcome, delay, retryCount, context) =>
                    {
                        log.Add($"Retry {retryCount} after {delay}");
                    });

            try
            {
                var response = await retryPolicy.ExecuteAsync(() =>
                    _httpClient.GetAsync("https://httpstat.us/500?sleep=500"));
                log.Add($"Response: {response.StatusCode}");
                return log;
            }
            catch (Exception ex)
            {
                log.Add($"Exception: {ex.Message}");
                return log;
            }
        }


        public async Task<object> SendManyRequestsToEnableBulkhead()
        {
            // TODO: Expor os logs via API para demonstrar claramente como a política de Bulkhead limita a concorrência e trata rejeições.
            var bulkheadPolicy = Policy
                .BulkheadAsync<HttpResponseMessage>(1, 5);

            var tasks = Enumerable.Range(0, 15).Select(async i =>
            {
                try
                {
                    var response = await bulkheadPolicy.ExecuteAsync(() => _httpClient.GetAsync("https://httpstat.us/200?sleep=500"));
                    return $"Task {i}: {response.StatusCode}";
                }
                catch (BulkheadRejectedException ex)
                {
                    _logger.LogWarning(ex, $"Task {i}: Rejeitada pelo Bulkhead");
                    return $"Task {i}: Rejeitada pelo Bulkhead";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Task {i}: Falha geral");
                    return $"Task {i}: Falha geral";
                }
            });

            var results = await Task.WhenAll(tasks);
            return results;
        }

    }
}
