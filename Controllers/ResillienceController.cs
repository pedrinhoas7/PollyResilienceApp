using Microsoft.AspNetCore.Mvc;
using PollyResilienceApp.Configurations;
using PollyResilienceApp.Interfaces;

namespace PollyResilienceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResillienceController : ControllerBase
    {
        private readonly IResillienceService _service;

        public ResillienceController(IResillienceService service)
        {
            _service = service;
        }

        [HttpGet("Simulate/RetryAndCircuitBreak")]
        public async Task<string> GetInternalServeError()
        {
            return await _service.GetInternalServeError();
        }

        [HttpGet("Simulate/Bulkhead")]
        public async Task<IActionResult> SendManyRequests()
        {
            var result = await _service.SendManyRequests();
            return Ok(result);
        }
    }
}
