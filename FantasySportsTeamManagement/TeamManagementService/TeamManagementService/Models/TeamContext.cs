using Microsoft.EntityFrameworkCore;

namespace TeamManagementService.Models
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<TeamContext> options)
            : base(options)
        {
        }

        public DbSet<Team> teams { get; set; }
    }
}
