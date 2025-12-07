using Microsoft.AspNetCore.Mvc;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        // GET: api/players
        // For now this just returns a placeholder message
        // We'll hook this up to the database later
        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok("Player list placeholder from PlayerManagementService");
        }
    }
}
