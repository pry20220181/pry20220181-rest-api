﻿using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_data_layer.Blockchain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Vaccination
{
    public class AdministeredDoseRepository : IAdministeredDoseRepository
    {
        PRY20220181DbContext _dbContext { get; set; }
        IBlockchainClient _blockchainClient { get; set; }

        public AdministeredDoseRepository(PRY20220181DbContext dbContext, IBlockchainClient blockchainClient)
        {
            _dbContext = dbContext;
            this._blockchainClient = blockchainClient;
        }

        public async Task<List<AdministeredDose>> GetByChildIdAsync(int childId)
        {
            return await _blockchainClient.GetByChildIdAsync(childId);
        }

        public async Task<string> CreateAsync(AdministeredDose administeredDose)
        {
            return await _blockchainClient.CreateAsync(administeredDose);
        }

        public async Task<List<AdministeredDose>> GetByChildIdWithAllRelatedInfoAsync(int childId)
        {
            var administeredDoses = await _blockchainClient.GetByChildIdAsync(childId);

            var dosesDetailIds = administeredDoses.Select(d => d.DoseDetailId);
            var dosesDetails = await _dbContext.DosesDetails
                .Include(d => d.VaccinationSchemeDetail.VaccinationScheme)
                .Include(d => d.VaccinationSchemeDetail.Vaccine)
                .Where(d => dosesDetailIds.Contains(d.DoseDetailId))
                .ToListAsync();

            var healthCenterIds = administeredDoses.Select(d => d.HealthCenterId);
            var healthCenters = await _dbContext.HealthCenters.Where(h => healthCenterIds.Contains(h.HealthCenterId)).ToListAsync();

            var healthPersonnelIds = administeredDoses.Select(d => d.HealthPersonnelId);
            var healthPersonnels = await _dbContext.HealthPersonnel
                .Include(h => h.User)
                .Where(h => healthPersonnelIds.Contains(h.HealthPersonnelId)).ToListAsync();

            foreach (var administeredDose in administeredDoses)
            {
                administeredDose.DoseDetail = dosesDetails.FirstOrDefault(d => d.DoseDetailId == administeredDose.DoseDetailId);
                administeredDose.HealthCenter = healthCenters.FirstOrDefault(h => h.HealthCenterId == administeredDose.HealthCenterId);
                administeredDose.HealthPersonnel = healthPersonnels.FirstOrDefault(h => h.HealthPersonnelId == administeredDose.HealthPersonnelId);
            }

            return administeredDoses;
        }

        public async Task<List<AdministeredDose>> GetByDosesIdList(int childId, List<int> doseDetailIds)
        {
            var administeredDoses = await _blockchainClient.GetByChildIdAsync(childId);

            return administeredDoses.Where(a => doseDetailIds.Contains(a.DoseDetailId)).ToList();
        }

        public async Task<AdministeredDose> GetAdministeredDoseByIdAsync(string administeredDoseId)
        {
            var administeredDose = await _blockchainClient.GetAdministeredDoseByIdAsync(administeredDoseId);

            var dosesDetails = await _dbContext.DosesDetails
                .Include(d => d.VaccinationSchemeDetail.VaccinationScheme)
                .Include(d => d.VaccinationSchemeDetail.Vaccine)
                .Where(d => d.DoseDetailId == administeredDose.DoseDetailId)
                .ToListAsync();

            var healthCenter = await _dbContext.HealthCenters.FirstOrDefaultAsync(h => h.HealthCenterId == administeredDose.HealthCenterId);

            var healthPersonnel = await _dbContext.HealthPersonnel
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.HealthPersonnelId == administeredDose.HealthPersonnelId);

                administeredDose.DoseDetail = dosesDetails.FirstOrDefault(d => d.DoseDetailId == administeredDose.DoseDetailId);
                administeredDose.HealthCenter = healthCenter;
                administeredDose.HealthPersonnel = healthPersonnel;

            return administeredDose;
        }
    }
}
