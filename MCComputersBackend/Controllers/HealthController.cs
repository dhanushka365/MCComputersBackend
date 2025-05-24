using Microsoft.AspNetCore.Mvc;

namespace MCComputersBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "MCComputers Backend API",
                Version = "1.0.0"
            });
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailedHealth()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "MCComputers Backend API",
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                WorkingSet = Environment.WorkingSet,
                Dependencies = new
                {
                    Database = "InMemory - Healthy",
                    EntityFramework = "Healthy"
                }
            });
        }
    }
}
