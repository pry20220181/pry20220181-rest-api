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
            if((await _dbContext.Parents.FirstOrDefaultAsync(p=>p.DNI == parent.DNI)) is null)
            {
                var newParent = await _dbContext.Parents.AddAsync(parent);
                await _dbContext.SaveChangesAsync();
                return newParent.Entity.ParentId;
            }
            else
            {
                throw new Exception($"A Parent with DNI {parent.DNI} already exists");
            }
        }

        public async Task<List<Child>> GetChildrenAsync(int parentId)
        {
            var children = await _dbContext.ChildrenParents
                .Include(p => p.Child)
                .Where(p => p.ParentId == parentId)
                .ToListAsync();

            return children.Select(p => new Child()
            {
                ChildId = p.ChildId,
                Firstname = p.Child.Firstname,
                Lastname = p.Child.Lastname,
                DNI = p.Child.DNI,
                Birthdate = p.Child.Birthdate,
                Gender = p.Child.Gender
            }).ToList();
        }

        public async Task<Parent> GetByIdAsync(int parentId)
        {
            return await _dbContext.Parents
                .Include(p=>p.User)
                .FirstOrDefaultAsync(p => p.ParentId == parentId);
        }

        public async Task<List<Parent>> GetAllByUbigeoIds(List<int> ubigeoIds)
        {
            return await _dbContext.Parents
                .Include(p=>p.User)
                .Where(p => ubigeoIds.Contains(p.UbigeoId)).ToListAsync();
        }
    }
}
