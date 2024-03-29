﻿using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IAdministeredDoseRepository
    {
        /// <summary>
        /// Get the Administered Doses of the Specified Child
        /// </summary>
        /// <param name="childId"></param>
        /// <returns>The List of Administered Dose</returns>
        public Task<List<AdministeredDose>> GetByChildIdAsync(int childId);
        /// <summary>
        /// Get the Administered Doses of the Specified Child with all its related Info (Doses, Schemes, Health Centers and Personnel)
        /// </summary>
        /// <param name="childId"></param>
        /// <returns>The List of Administered Dose with all its related Info</returns>
        public Task<List<AdministeredDose>> GetByChildIdWithAllRelatedInfoAsync(int childId);
        public Task<List<AdministeredDose>> GetByDosesIdList(int childId, List<int> doseDetailIds);
        public Task<AdministeredDose> GetAdministeredDoseByIdAsync(string administeredDoseId);
        public Task<string> CreateAsync(AdministeredDose administeredDose);
    }
}
