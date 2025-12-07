using Microsoft.AspNetCore.Mvc;
using PlayerManagementService.Models;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        // Temporary in-memory sample players
        private static List<Player> samplePlayers = new List<Player>
        {
            new Player { PlayerId = 1, Name = "John Doe", Position = "Forward", TeamId = null, IsDrafted = false },
            new Player { PlayerId = 2, Name = "Alex Smith", Position = "Guard", TeamId = null, IsDrafted = false }
        };

        // GET: api/players
        // For now this just returns a placeholder message
        // We'll hook this up to the database later

        [HttpGet]
        public IActionResult GetPlayers()
        {

            return Ok("Player list placeholder from PlayerManagementService");
        }

        // POST: api/players
        [HttpPost]
        public IActionResult CreatePlayer([FromBody] Player newPlayer)
        {
            if (newPlayer == null)
            {
                return BadRequest("Player data is required.");
            }
            // Auto-increment PlayerId for now
            newPlayer.PlayerId = samplePlayers.Count + 1;

            // Add to in-memory list
            samplePlayers.Add(newPlayer);

            return CreatedAtAction(nameof(GetPlayers), new { id = newPlayer.PlayerId }, newPlayer);
        }

        // GET: api/players/{id}
        [HttpGet("{id}")]
        public ActionResult<Player> GetPlayerById(int id)
        {
            var player = samplePlayers.FirstOrDefault(p => p.PlayerId == id);

            if (player == null)
                return NotFound();

            return Ok(player);
        }

        // DELETE: api/players/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var player = samplePlayers.FirstOrDefault(p => p.PlayerId == id);

            if (player == null)
                return NotFound();

            samplePlayers.Remove(player);

            return NoContent();
        }

        // UPDATE a player (PUT)
        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            var existingPlayer = samplePlayers.FirstOrDefault(p => p.PlayerId == id);
            if (existingPlayer == null)
                return NotFound();

            // Update fields
            existingPlayer.Name = updatedPlayer.Name;
            existingPlayer.Position = updatedPlayer.Position;
            existingPlayer.TeamId = updatedPlayer.TeamId;
            existingPlayer.IsDrafted = updatedPlayer.IsDrafted;

            return Ok(existingPlayer);
        }


    }
}
