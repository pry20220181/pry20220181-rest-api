﻿using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories
{
    public class VaccineRepository : IVaccineRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public VaccineRepository(PRY20220181DbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public async Task<List<Vaccine>> GetAsync()
        {
            return await _dbContext.Vaccines.ToListAsync();
        }

        public async Task<Vaccine> GetByIdAsync(int id)
        {
            return await _dbContext.Vaccines.FindAsync(id);
        }

        public async Task<int> CreateAsync(Vaccine vaccine)
        {
            var createdVaccine = await _dbContext.Vaccines.AddAsync(vaccine);

            await _dbContext.SaveChangesAsync();

            return createdVaccine.Entity.Id;
        }

        public async Task<Vaccine> UpdateAsync(Vaccine vaccine)
        {
            var vaccineInDb = await _dbContext.Vaccines.FindAsync(vaccine.Id);

            vaccineInDb.Name = vaccine.Name;
            vaccineInDb.Description = vaccine.Description;

            await _dbContext.SaveChangesAsync();

            return vaccineInDb;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return false;
        }
    }
}
