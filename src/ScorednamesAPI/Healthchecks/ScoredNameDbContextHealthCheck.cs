using App.Metrics.Health;
using scorednameAPI.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace scorednameAPI.Healthchecks
{
    public class ScoredNameDbContextHealthCheck : HealthCheck
    {
        private ScoredNameContext _context;
        private const string MESSAGE = "Verifying valid DBContext exists";
    
        public ScoredNameDbContextHealthCheck(ScoredNameContext context) : base("ScoredName DbContext shallow health check.")
        {
            this._context = context;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this._context.ScoredNames.Count() >=0 || this._context.ScoredNames.Any())
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(MESSAGE));
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(MESSAGE));
        }
    }
}
