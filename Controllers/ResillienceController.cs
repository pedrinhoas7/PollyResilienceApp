using Microsoft.AspNetCore.Mvc;
using PollyResilienceApp.Configurations;
using PollyResilienceApp.Interfaces;

namespace PollyResilienceApp.Controllers
{
    [Route("")]
    [ApiController]
    public class ResillienceController : ControllerBase
    {
        private readonly IResillienceService _service;

        public ResillienceController(IResillienceService service)
        {
            _service = service;
        }

        [HttpGet("Simulate/RetryAndCircuitBreak")]
        public async Task<object> RetryAndCircuitBreak()
        {
            return await _service.RetryAndCircuitBreak();
        }

        [HttpGet("Simulate/Retry")]
        public async Task<object> ForceRetry()
        {
            return await _service.ForceRetry();
        }

        [HttpGet("Simulate/Bulkhead")]
        public async Task<IActionResult> SendManyRequestsToEnableBulkhead()
        {
            var result = await _service.SendManyRequestsToEnableBulkhead();
            return Ok(result);
        }
    }
}
