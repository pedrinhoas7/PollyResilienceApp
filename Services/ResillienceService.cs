using Polly.Bulkhead;
using PollyResilienceApp.Configurations;
using PollyResilienceApp.Interfaces;
using Polly;
using Microsoft.Extensions.Logging;

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

        public async Task<string> GetInternalServeError()
        {
            try
            {
                return await _client.GetInternalServeError();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> SendManyRequests()
        {
            try
            {
                // Forçando as requisições a terem mais concorrência
                var tasks = Enumerable.Range(0, 10).Select(i =>
                {
                    return Task.Run(() =>
                    {
                        return _httpClient.GetAsync("https://httpstat.us/200?sleep=500")
                            .ContinueWith(task =>
                            {
                                if (task.IsFaulted)
                                {
                                    if (task.Exception is AggregateException agEx)
                                    {
                                        foreach (var ex in agEx.InnerExceptions)
                                        {
                                            _logger.LogError(ex, $"Task {i}: Falha geral");
                                        }
                                    }
                                    return $"Task {i}: Falha geral";
                                }

                                if (task.IsCompletedSuccessfully)
                                {
                                    var response = task.Result;
                                    return $"Task {i}: {response.StatusCode}";
                                }

                                return $"Task {i}: Falhou de forma inesperada";
                            });
                    });
                }).ToArray();

                // Aguardando todas as tarefas
                return Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
