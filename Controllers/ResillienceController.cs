using Microsoft.AspNetCore.Mvc;
using PollyResilienceApp.Configurations;

namespace PollyResilienceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResillienceController : ControllerBase
    {


        private readonly ILogger<ResillienceController> _logger;
        private readonly IHttpClient _client;

        public ResillienceController(IHttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            return await _client.GetAll();
        }
    }
}
