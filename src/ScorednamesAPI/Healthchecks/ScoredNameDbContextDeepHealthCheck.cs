using App.Metrics.Health;
using scorednameAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace scorednameAPI.Healthchecks
{
    public class ScoredNameDbContextDeepHealthCheck : HealthCheck 
    {
        private ScoredNameContext _context;
        private const string MESSAGE = "Verifying adding and deleting on the DbContext";

        private ScoredName createDummyName()
        {
            var rnd = new Random();
            return new ScoredName { Name = "someRandomName", Score = rnd.Next(1000) };
        }

        private bool canAddDeleteFromDB()
        {
            bool success = false;
            var dummyName = createDummyName();
            var currentSize = this._context.ScoredNames.Count();

            // add a new item
            this._context.ScoredNames.Add(dummyName);
            this._context.SaveChanges();
            success = currentSize < this._context.ScoredNames.Count();

            // delete the item
            this._context.ScoredNames.Remove(dummyName);
            this._context.SaveChanges();
            success = currentSize == this._context.ScoredNames.Count();

            return success;
        }

        public ScoredNameDbContextDeepHealthCheck(ScoredNameContext context) : base("ScoredName DbContext deep health check.")
        {
            this._context = context;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
           if (this.canAddDeleteFromDB())
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(MESSAGE));
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(MESSAGE));
        }

        
    }
}
