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
        private readonly HttpClient _teamClient;

        public PlayersController(
            PlayerContext context,
            IHttpClientFactory httpClientFactory
        )
        {
            _context = context;
            _teamClient = httpClientFactory.CreateClient("TeamService");
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
                return BadRequest("Player data is required.");

            if (string.IsNullOrWhiteSpace(newPlayer.Name))
                return BadRequest("Player name is required.");

            if (string.IsNullOrWhiteSpace(newPlayer.Position))
                return BadRequest("Player position is required.");

            if (newPlayer.TeamId < 0)
                return BadRequest("TeamId cannot be negative.");

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

        // UPDATE a player (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            var existingPlayer = await _context.Players.FindAsync(id);
            if (existingPlayer == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(updatedPlayer.Name))
                return BadRequest("Player name is required.");

            if (string.IsNullOrWhiteSpace(updatedPlayer.Position))
                return BadRequest("Player position is required.");

            if (updatedPlayer.TeamId < 0)
                return BadRequest("TeamId cannot be negative.");

            // Update fields
            existingPlayer.Name = updatedPlayer.Name;
            existingPlayer.Position = updatedPlayer.Position;
            existingPlayer.TeamId = updatedPlayer.TeamId;
            existingPlayer.IsDrafted = updatedPlayer.IsDrafted;

            await _context.SaveChangesAsync();

            return Ok(existingPlayer);
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


        // GET: api/players/drafted
        [HttpGet("drafted")]
        public async Task<IActionResult> GetDraftedPlayers()
        {
            var draftedPlayers = await _context.Players
                .Where(p => p.IsDrafted == true)
                .ToListAsync();

            return Ok(draftedPlayers);
        }


        // GET: api/players/undrafted
        [HttpGet("undrafted")]
        public async Task<IActionResult> GetUndraftedPlayers()
        {
            var undraftedPlayers = await _context.Players
                .Where(p => p.IsDrafted == false)
                .ToListAsync();

            return Ok(undraftedPlayers);
        }

        // GET: api/players/search?name=John
        [HttpGet("search")]
        public async Task<IActionResult> SearchPlayers(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("A name query is required.");

            var results = await _context.Players
                .Where(p => p.Name.Contains(name))
                .ToListAsync();

            return Ok(results);
        }


        // GET: api/players/by-team/{teamId}
        [HttpGet("by-team/{teamId}")]
        public async Task<IActionResult> GetPlayersByTeam(int teamId)
        {
            var players = await _context.Players
                .Where(p => p.TeamId == teamId)
                .ToListAsync();

            return Ok(players);
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

    }
}
