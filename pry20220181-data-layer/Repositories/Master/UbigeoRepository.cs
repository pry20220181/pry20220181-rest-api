using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class UbigeoRepository : IUbigeoRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public UbigeoRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Ubigeo>> GetUbigeosAsync()
        {
            return await _dbContext.Ubigeo.ToListAsync();
        }
    }
}
