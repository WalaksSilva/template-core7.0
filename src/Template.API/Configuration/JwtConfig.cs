using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Template.API.Configuration
{
    public static class JwtConfig
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"]);
                options.Authority = configuration["Authentication:Authority"];
                options.IncludeErrorDetails = bool.Parse(configuration["Authentication:IncludeErrorDetails"]);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = bool.Parse(configuration["Authentication:ValidateAudience"]),
                    ValidAudience = configuration["Authentication:ValidAudience"],
                    ValidateIssuerSigningKey = bool.Parse(configuration["Authentication:ValidateIssuerSigningKey"]),
                    ValidateIssuer = bool.Parse(configuration["Authentication:ValidateIssuer"]),
                    ValidIssuer = configuration["Authentication:ValidIssuer"],
                    ValidateLifetime = bool.Parse(configuration["Authentication:ValidateLifetime"])
                };
            });

            return services;
        }
    }
}
