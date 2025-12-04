using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private static readonly List<Team> teams = new List<Team> { };
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> Get()
        {
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> Get(int id)
        {
            var team = teams.FirstOrDefault(x => x.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }
        [HttpPost]
        public ActionResult AddTeam(Team addedTeam)
        {
            try
            {
                if (teams.Count > 0)
                {
                    addedTeam.TeamId = teams.Max(u => u.TeamId) + 1;
                }
                teams.Add(addedTeam);
                return Ok("Team added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //delete
        [HttpDelete("{deletedID}")]
        public ActionResult DeleteTeam(int deletedID)
        {
            try
            {
                var team = teams.FirstOrDefault(x => x.TeamId == deletedID);
                if (team == null)
                {
                    return NotFound("Thats not a real team.");
                }
                teams.Remove(team);
                return Ok("Retconned team existence.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
