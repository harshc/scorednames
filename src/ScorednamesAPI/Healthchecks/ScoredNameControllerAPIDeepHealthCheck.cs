using App.Metrics.Health;
using scorednameAPI.Controllers;
using scorednameAPI.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace scorednameAPI.Healthchecks
{
    public class ScoredNameControllerAPIDeepHealthCheck : HealthCheck
    {
        private ScoredNameContext _context;
        private ScoredNameController _apiController;
        private const string MESSAGE = "Verifying the /api/scoredname endpoint is healthy";

        private ScoredName createDummyName()
        {
            var rnd = new Random();
            return new ScoredName { Name = "someRandomName", Score = rnd.Next(1000) };
        }

        public ScoredNameControllerAPIDeepHealthCheck(ScoredNameContext context) : base("ScoredName API Deep Health Check")
        {
            _context = context;
            _apiController = new ScoredNameController(this._context);
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            bool success = false;
            if (_context == null || _apiController == null)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(MESSAGE));
            }

            // GET api/scoredname
            var getAllResult = _apiController.GetAll();
            success = (getAllResult != null && getAllResult.Count() >= 0);

            // GET api/scoredname/{id}
            var dummy = createDummyName();
            _context.ScoredNames.Add(dummy);
            _context.SaveChanges();
            var getbyIdresult = _apiController.GetById(_context.ScoredNames.First().Id);
            success &= getbyIdresult != null;
            _context.ScoredNames.Remove(dummy);
            _context.SaveChanges();

            // POST api/scoredname
            // TODO: add a POST check here
            if (success) {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(MESSAGE));
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(MESSAGE));
        }

    }
}
