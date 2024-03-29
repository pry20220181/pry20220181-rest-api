﻿using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Blockchain
{
    public interface IBlockchainClient
    {
        public Task<List<AdministeredDose>> GetByChildIdAsync(int childId);
        public Task<AdministeredDose> GetAdministeredDoseByIdAsync(string id);
        public Task<string> CreateAsync(AdministeredDose administeredDose);
    }
}
