using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class UserRepository : IUserRepository
    {
        PRY20220181DbContext _dbContext { get; set; }
        private ILogger<UserRepository> _logger { get; set; }

        public UserRepository(PRY20220181DbContext dbContext, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<HealthPersonnel> GetHealthPersonnelByUserIdAsync(string userId)
        {
            return await _dbContext.HealthPersonnel.FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task<Parent> GetParentByUserIdAsync(string userId)
        {
            return await _dbContext.Parents.FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
