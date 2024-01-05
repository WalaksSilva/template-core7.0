using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Template.API.Configuration
{
    public static class HealthConfig
    {
        public static IServiceCollection AddHealthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (PlatformServices.Default.Application.ApplicationName != "testhost")
            {
                var healthCheck = services.AddHealthChecksUI(setupSettings: setup =>
                {
                    setup.DisableDatabaseMigrations();
                    setup.MaximumHistoryEntriesPerEndpoint(6);
                    //setup.AddWebhookNotification("Teams", configuration["Webhook:Teams"],
                    //    payload: System.IO.File.ReadAllText(Path.Combine(".", "MessageCard", "ServiceDown.json")),
                    //    restorePayload: System.IO.File.ReadAllText(Path.Combine(".", "MessageCard", "ServiceRestore.json")),
                    //    customMessageFunc: (str, report) =>
                    //    {
                    //        var failing = report.Entries.Where(e => e.Value.Status == UIHealthStatus.Unhealthy);
                    //        return $"{AppDomain.CurrentDomain.FriendlyName}: {failing.Count()} healthchecks are failing";
                    //    });
                }).AddInMemoryStorage();

                var builder = healthCheck.Services.AddHealthChecks();

                //500 mb
                builder.AddProcessAllocatedMemoryHealthCheck(500 * 1024 * 1024, "Process Memory", tags: new[] { "self" });
                //500 mb
                builder.AddPrivateMemoryHealthCheck(1500 * 1024 * 1024, "Private memory", tags: new[] { "self" });

                //builder.AddSqlServer(Configuration["ConnectionStrings:DefaultConnection"], tags: new[] { "services" });

                //dotnet add <Project> package AspNetCore.HealthChecks.OpenIdConnectServer
                //builder.AddIdentityServer(new Uri(Configuration["Authentication:Authority"]), "SSO Inova", tags: new[] { "services" });

                builder.AddApplicationInsightsPublisher();
            }

            return services;
        }

        public static IEndpointRouteBuilder AddHealthChecksEndpoint(this IEndpointRouteBuilder endpoints)
        {
            if (PlatformServices.Default.Application.ApplicationName != "testhost")
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("self"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("services"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/health-ui";
                });
            }

            return endpoints;
        }
    }
}
