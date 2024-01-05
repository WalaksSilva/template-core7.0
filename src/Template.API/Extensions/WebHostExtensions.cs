using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Infra.Context;
using Microsoft.Extensions.Hosting;

namespace Inova.MetaVerso.API.Extensions;

[ExcludeFromCodeCoverage]
public static class WebHostExtensions
{
    public static IHost SeedData(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<EntityContext>();

            context.Database.Migrate();

            new EntityContextSeed(context);
        }

        return host;
    }
}