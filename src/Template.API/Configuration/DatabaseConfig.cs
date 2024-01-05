using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data.SqlClient;
using Template.Infra.Context;

namespace Template.API.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<EntityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CustomerDB")));
            services.AddSingleton<DbConnection>(conn => new SqlConnection(configuration.GetConnectionString("CustomerDB")));
            services.AddScoped<DapperContext>();

            return services;
        }
    }
}
