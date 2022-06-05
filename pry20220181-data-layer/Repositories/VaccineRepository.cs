using Microsoft.EntityFrameworkCore;
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


        public async Task<List<Vaccine>> Get()
        {
            return await _dbContext.Vaccines.ToListAsync();
        }

        public async Task<Vaccine> GetById(int id)
        {
            return await _dbContext.Vaccines.FindAsync(id);
        }

        public Vaccine Create(Vaccine vaccine)
        {
            return null;
        }

        public Vaccine Update(Vaccine vaccine)
        {
            return null;
        }

        public bool Delete(int id)
        {
            return false;
        }
    }
}
