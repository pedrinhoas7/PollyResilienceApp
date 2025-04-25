using Refit;

namespace PollyResilienceApp.Configurations
{
    public interface IHttpClient
    {
        [Get("/todos/2")]
        Task<object> GetAll();
    }
}
