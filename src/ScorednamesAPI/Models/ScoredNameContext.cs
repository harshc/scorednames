using App.Metrics.Health;
using Microsoft.EntityFrameworkCore;
using scorednameAPI.Healthchecks;

namespace scorednameAPI.Models
{
    public class ScoredNameContext : DbContext
    {
        // private readonly ScoredNameDbContextHealthCheck _healthCheck;
        public ScoredNameContext(DbContextOptions<ScoredNameContext> options) : base(options)
        {
        }

        public DbSet<ScoredName> ScoredNames { get; set; }
    }
}
