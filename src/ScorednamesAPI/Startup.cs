using App.Metrics;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace scorednameAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var metrics = AppMetrics.CreateDefaultBuilder().Build();
            var health = AppMetricsHealth.CreateDefaultBuilder().HealthChecks.RegisterFromAssembly(services);
            
            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();

            services.AddHealth(health);
            services.AddHealthEndpoints();

            services.AddDbContext<Models.ScoredNameContext>(opt => opt.UseInMemoryDatabase("IndexItems"), ServiceLifetime.Singleton);
            services.AddMvc(options => options.AddMetricsResourceFilter());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMetricsAllMiddleware();
            app.UseHealthAllEndpoints();
            app.UseMvc();
        }
    }
}
