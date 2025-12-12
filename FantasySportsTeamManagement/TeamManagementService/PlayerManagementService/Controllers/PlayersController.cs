using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Models;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayersController(PlayerContext context)
        {
            _context = context;
        }

        // GET: api/players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }


        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer([FromBody] Player newPlayer)
        {
            if (newPlayer == null)
            {
                return BadRequest("Player data is required.");
            }

            // EF Core saves the player to the database
            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = newPlayer.PlayerId }, newPlayer);
        }

        // GET: api/players/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound();

            return player;
        }


        // DELETE: api/players/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DRAFT a player
        [HttpPost("{id}/draft")]
        public async Task<IActionResult> DraftPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            player.IsDrafted = true;

            await _context.SaveChangesAsync();

            return Ok(player);
        }

        // RELEASE a player
        [HttpPost("{id}/release")]
        public async Task<IActionResult> ReleasePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            player.IsDrafted = false;
            player.TeamId = null; //clears the team

            await _context.SaveChangesAsync();

            return Ok(player);
        }


        // UPDATE a player (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            var existingPlayer = await _context.Players.FindAsync(id);
            if (existingPlayer == null)
                return NotFound();

            // Update fields
            existingPlayer.Name = updatedPlayer.Name;
            existingPlayer.Position = updatedPlayer.Position;
            existingPlayer.TeamId = updatedPlayer.TeamId;
            existingPlayer.IsDrafted = updatedPlayer.IsDrafted;

            await _context.SaveChangesAsync();

            return Ok(existingPlayer);
        }


    }
}
