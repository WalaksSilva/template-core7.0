using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.Net.Http.Headers;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.IO.Compression;
using System.Linq;
using System.Text.Json.Serialization;
using Template.API.Configuration;
using Template.API.Extensions;
using Template.API.Filters;
using Template.API.Middlewares;

namespace Template.API;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        WebHostEnvironment = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment WebHostEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = true;
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        services.AddControllers();
        services.AddMvc(options =>
        {
            options.Filters.Add<DomainNotificationFilter>();
            options.EnableEndpointRouting = false;
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddJwtConfiguration(Configuration);

        services.Configure<TelemetryConfiguration>((o) =>
        {
            o.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
        });
        services.Configure<GzipCompressionProviderOptions>(x => x.Level = CompressionLevel.Optimal);
        services.AddResponseCompression(x =>
        {
            x.Providers.Add<GzipCompressionProvider>();
        });

        services.RegisterHttpClient(Configuration);
        services.AddHealthConfiguration(Configuration);

        if (!WebHostEnvironment.IsProduction())
        {
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1";
                document.Version = "v1";
                document.Title = "Template API";
                document.Description = "API de Template";
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = HeaderNames.Authorization,
                    Description = "Token de autenticação via SSO",
                    In = OpenApiSecurityApiKeyLocation.Header
                });

                document.PostProcess = (configure) =>
                {
                    configure.Info.TermsOfService = "None";
                    configure.Info.Contact = new OpenApiContact()
                    {
                        Name = "Squad",
                        Email = "squad@xyz.com",
                        Url = "exemplo.xyz.com"
                    };
                    configure.Info.License = new OpenApiLicense()
                    {
                        Name = "Exemplo",
                        Url = "exemplo.xyz.com"
                    };
                };


            });
        }

        services.AddAutoMapper(typeof(Startup));
        services.AddHttpContextAccessor();
        services.AddApplicationInsightsTelemetry();

        services.RegisterServices(Configuration);
        services.RegisterDatabaseServices(Configuration);
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, TelemetryClient telemetryClient)
    {
        if (!env.IsProduction())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseResponseCompression();

        if (!env.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseLogMiddleware();

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = new ErrorHandlerMiddleware(telemetryClient, env).Invoke
        });

        app.UseEndpoints(endpoints =>
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

            endpoints.MapControllers();
        });
    }
}
