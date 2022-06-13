using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Services;
using pry20220181_core_layer.Modules.Vaccination.Services.Impl;
using pry20220181_data_layer.Repositories;
using pry20220181_data_layer.Repositories.Master;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtension
    {
        /// <summary>
        /// Configure dependency injection for the PRY20220181's Repositories
        /// Check out the code to know the configured repositories.
        /// </summary>
        /// <param name="services"></param>
        public static void AddPRY20220181Repositories(this IServiceCollection services)
        {
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IHealthPersonnelRepository, HealthPersonnelRepository>();
        }

        /// <summary>
        /// Configure dependency injection for the PRY20220181's Services
        /// Check out the code to know the configured services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddPRY20220181Services(this IServiceCollection services)
        {
            services.AddScoped<IVaccineService, VaccineService>();
        }
    }
}
