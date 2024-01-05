using Microsoft.Extensions.DependencyInjection;
using Template.API.Services.Interfaces;
using Template.API.Services;
using Template.API.Settings;
using Template.Domain.Interfaces.Identity;
using Template.Domain.Interfaces.Notifications;
using Template.Domain.Interfaces.Repository;
using Template.Domain.Interfaces.UoW;
using Template.Domain.Notifications;
using Template.Infra.Identity;
using Template.Infra.Repository;
using Template.Infra.UoW;
using Microsoft.Extensions.Configuration;

namespace Template.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationInsightsSettings>(configuration.GetSection("ApplicationInsights"));

            #region Service
            services.AddScoped<ICustomerService, CustomerService>();

            #endregion

            #region Domain

            services.AddScoped<IDomainNotification, DomainNotification>();

            #endregion

            #region Infra

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IIdentityService, IdentityService>();

            #endregion

            return services; ;
        }
    }
}
