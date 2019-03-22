namespace Clarity.Services
{
    using Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(_configuration)
                .Configure<EmailOptions>(_configuration.GetSection(nameof(EmailOptions)))
                .Configure<ServiceBusOptions>(_configuration.GetSection(nameof(ServiceBusOptions)))
                .AddSingleton<IEmailService, SendGridEmailService>()
                .AddHostedService<EmailQueueClient>()
                .AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");
        }
    }
}
