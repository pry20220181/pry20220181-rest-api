using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class ParentRepository : IParentRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public ParentRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateWithChildrenAsync(Parent parent)
        {
            var newParent = await _dbContext.Parents.AddAsync(parent);
            await _dbContext.SaveChangesAsync();
            return newParent.Entity.ParentId;
        }
    }
}
