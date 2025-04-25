using Microsoft.AspNetCore.Mvc;
using PollyResilienceApp.Configurations;

namespace PollyResilienceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClient _client;

        public WeatherForecastController(IHttpClient client)
        {
            _client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<object> Get()
        {
            return await _client.GetAll();
        }
    }
}
