﻿using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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


        public async Task<List<Vaccine>> GetAsync(PaginationParameter paginationParameter)
        {
            return await _dbContext.Vaccines
                .Skip(paginationParameter.PageSize*(paginationParameter.Page-1))
                .Take(paginationParameter.PageSize)
                .ToListAsync();
        }

        public async Task<Vaccine> GetByIdAsync(int id)
        {
            return await _dbContext.Vaccines.FindAsync(id);
        }

        public async Task<int> CreateAsync(Vaccine vaccine)
        {
            var createdVaccine = await _dbContext.Vaccines.AddAsync(vaccine);

            await _dbContext.SaveChangesAsync();

            return createdVaccine.Entity.VaccineId;
        }

        public async Task<Vaccine> UpdateAsync(Vaccine vaccine)
        {
            var vaccineInDb = await _dbContext.Vaccines.FindAsync(vaccine.VaccineId);

            vaccineInDb.Name = vaccine.Name;
            vaccineInDb.Description = vaccine.Description;

            await _dbContext.SaveChangesAsync();

            return vaccineInDb;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vaccineInDb = await _dbContext.Vaccines.FindAsync(id);

            if (vaccineInDb == null)
                return false;

            _dbContext.Vaccines.Remove(vaccineInDb);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
