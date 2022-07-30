using pry20220181_core_layer.Modules.Campaigns.Repositories;
using pry20220181_core_layer.Modules.Campaigns.Services;
using pry20220181_core_layer.Modules.Campaigns.Services.Impl;
using pry20220181_core_layer.Modules.Inventory.Repositories;
using pry20220181_core_layer.Modules.Inventory.Services;
using pry20220181_core_layer.Modules.Inventory.Services.Impl;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Master.Services.Impl;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Services;
using pry20220181_core_layer.Modules.Vaccination.Services.Impl;
using pry20220181_data_layer.Repositories.Campaigns;
using pry20220181_data_layer.Repositories.Inventory;
using pry20220181_data_layer.Repositories.Master;
using pry20220181_data_layer.Repositories.Vaccination;

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
            services.AddScoped<IAdministeredDoseRepository, AdministeredDoseRepository>();
            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<IDoseDetailRepository, DoseDetailRepository>();
            services.AddScoped<IHealthCenterRepository, HealthCenterRepository>();
            services.AddScoped<IHealthPersonnelRepository, HealthPersonnelRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVaccinationAppointmentRepository, VaccinationAppointmentRepository>();
            services.AddScoped<IVaccinationCampaignRepository, VaccinationCampaignRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IVaccinationSchemeRepository, VaccinationSchemeRepository>();
            services.AddScoped<IVaccinationSchemeDetailRepository, VaccinationSchemeDetailRepository>();
        }

        /// <summary>
        /// Configure dependency injection for the PRY20220181's Services
        /// Check out the code to know the configured services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddPRY20220181Services(this IServiceCollection services)
        {
            services.AddScoped<IChildService, ChildService>();
            services.AddScoped<IDosesService, DosesService>();
            services.AddScoped<IHealthCenterService, HealthCenterService>();
            services.AddScoped<IHealthPersonnelService, HealthPersonnelService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IReminderService, ReminderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVaccinationAppointmentService, VaccinationAppointmentService>();
            services.AddScoped<IVaccinationCampaignsService, VaccinationCampaignsService>();
            services.AddScoped<IVaccineService, VaccineService>();
        }
    }
}
