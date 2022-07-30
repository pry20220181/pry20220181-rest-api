using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Campaigns.Repositories;
using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Campaigns
{
    public class VaccinationCampaignRepository : IVaccinationCampaignRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public VaccinationCampaignRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VaccinationCampaign>> GetByHealthCenterIdAsync(int healthCenterId, DateTime sinceDate)
        {
            var locations = await _dbContext.VaccinationCampaignLocations
                .Include(l => l.VaccinationCampaign)
                .Where(x => x.HealthCenterId == healthCenterId && x.VaccinationCampaign.EndDateTime >= DateTime.Parse(sinceDate.ToShortDateString()))
                .ToListAsync();
            return locations.Select(l => l.VaccinationCampaign).ToList();
        }

        public async Task<VaccinationCampaign> GetByIdWithLocationsAndVaccinesAsync(int campaignId)
        {
            return await _dbContext.VaccinationCampaigns
                .Include(c => c.VaccinationCampaignLocations)
                    .ThenInclude(l => l.HealthCenter)
                .Include(c => c.VaccinationCampaignDetails)
                    .ThenInclude(d => d.Vaccine)
                .Where(c => c.VaccinationCampaignId == campaignId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateVaccinationCampaign(VaccinationCampaign vaccinationCampaign)
        {
            var createdCampaign = await _dbContext.VaccinationCampaigns.AddAsync(vaccinationCampaign);
            await _dbContext.SaveChangesAsync();
            return createdCampaign.Entity.VaccinationCampaignId;
        }

        public async Task<List<int>> GetUbigeosByVaccinationCampaignId(int vaccinationCampaignId)
        {
            var healthCenters = await 
                _dbContext.VaccinationCampaignLocations
                .Include(c=>c.VaccinationCampaign)
                .Include(c=>c.HealthCenter)
                .Where(v => v.VaccinationCampaignId == vaccinationCampaignId)
                .ToListAsync();
            var ubigeoIds = healthCenters.Select(h => h.HealthCenter.UbigeoId).ToList();
            return ubigeoIds;
        }
    }
}