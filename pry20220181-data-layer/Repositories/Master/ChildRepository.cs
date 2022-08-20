using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class ChildRepository : IChildRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public ChildRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Child> GetByDniAsync(string DNI)
        {
            return await _dbContext.Children.FirstOrDefaultAsync(c => c.DNI == DNI);
        }
            
        public async Task<Child> GetByIdAsync(int childId)
        {
            return await _dbContext.Children.FirstOrDefaultAsync(c => c.ChildId == childId);
        }

        public async Task<Child> GetByIdWithParentsIdAsync(int childId)
        {
            return await _dbContext.Children
                .Include(c => c.ChildParents)
                .FirstOrDefaultAsync(c => c.ChildId == childId);
        }

        public async Task<List<Child>> GetChildrenAsync()
        {
            return await _dbContext.Children.ToListAsync();
        }
    }
}
