using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.CircuitBreaker;
using Polly;
using System.Net.Http;
using System;
using Template.Domain.Interfaces.Services;
using Template.Infra.Services;
using System.Net.Http.Headers;
using System.Net;

namespace Template.API.Configuration
{
    public static class ClientConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IViaCEPService, ViaCEPService>((s, c) =>
            {
                c.BaseAddress = new Uri(configuration["API:ViaCEP"]);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        const string SleepDurationKey = "Broken";
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy<HttpResponseMessage>
                    .HandleResult(res => res.StatusCode == HttpStatusCode.GatewayTimeout || res.StatusCode == HttpStatusCode.RequestTimeout)
                    .Or<BrokenCircuitException>()
                    .WaitAndRetryAsync(4,
                        sleepDurationProvider: (c, ctx) =>
                        {
                            if (ctx.ContainsKey(SleepDurationKey))
                                return (TimeSpan)ctx[SleepDurationKey];
                            return TimeSpan.FromMilliseconds(200);
                        },
                        onRetry: (dr, ts, ctx) =>
                        {
                            Console.WriteLine($"Context: {(ctx.ContainsKey(SleepDurationKey) ? "Open" : "Closed")}");
                            Console.WriteLine($"Waits: {ts.TotalMilliseconds}");
                        });
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>
                .HandleResult(res => res.StatusCode == HttpStatusCode.GatewayTimeout || res.StatusCode == HttpStatusCode.RequestTimeout)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                   onBreak: (dr, ts, ctx) => { ctx[SleepDurationKey] = ts; },
                   onReset: (ctx) => { ctx[SleepDurationKey] = null; });
        }
    }
}
