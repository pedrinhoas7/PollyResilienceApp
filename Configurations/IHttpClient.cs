using Refit;

namespace PollyResilienceApp.Configurations
{
    public interface IHttpClient
    {
        [Get("/500")]
        Task<string> GetInternalServeError();

        [Get("/200?sleep=50000")]
        Task<string> GetOk();
    }
}
