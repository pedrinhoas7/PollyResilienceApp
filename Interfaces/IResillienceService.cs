namespace PollyResilienceApp.Interfaces
{
    public interface IResillienceService
    {
        public Task<string> GetInternalServeError();
        public Task<object> SendManyRequests();
    }
}
