using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Resume Scoring API is running...");
        }
    }
}
