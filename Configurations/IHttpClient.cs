using Refit;

namespace PollyResilienceApp.Configurations
{
    public interface IHttpClient
    {
        [Get("/500")]
        Task<string> GetInternalServeError();

        [Post("/201")]
        Task<string> GetOk();
    }
}
