using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TeamManagementService.Models;

namespace TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamContext _context;

        public TeamController(TeamContext context)
        {
            _context = context;
        }
        private static readonly List<Team> teams = new List<Team> { };
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> Get()
        {
            return await _context.teams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.teams.FindAsync(id);

            if (team == null)
                return NotFound();

            return team;
        }
        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] Team newTeam)
        {
            if (newTeam == null)
            {
                return BadRequest("Team data is required.");
            }

            // EF Core saves the team to the database
            _context.teams.Add(newTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = newTeam.TeamId }, newTeam);
        }

        //update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] Team updatedTeam)
        {
            var existingTeam = await _context.teams.FindAsync(id);
            if (existingTeam == null)
                return NotFound();

            // Update fields
            existingTeam.TeamId = updatedTeam.TeamId;
            existingTeam.TeamName = updatedTeam.TeamName;

            await _context.SaveChangesAsync();

            return Ok(existingTeam);
        }

        //delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.teams.FindAsync(id);

            if (team == null)
                return NotFound();

            _context.teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
