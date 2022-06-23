using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Inventory.Models;
using pry20220181_core_layer.Modules.Inventory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Inventory
{
    public class InventoryRepository : IInventoryRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public InventoryRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VaccineInventory>> GetAllByHealthCenterAsync(int healthCenterId)
        {
            return await _dbContext.VaccineInventory
                .Include(i=>i.Vaccine)
                .Include(i => i.HealthCenter)
                .Where(i => i.HealthCenterId == healthCenterId)
                .ToListAsync();
        }

        public async Task<VaccineInventory> GetByHealthCenterAndVaccineAsync(int healthCenterId, int vaccineId)
        {
            return await _dbContext.VaccineInventory
                    .Include(i => i.Vaccine)
                    .Include(i => i.HealthCenter)
                    .Where(i => i.HealthCenterId == healthCenterId && i.VaccineId == vaccineId)
                    .FirstOrDefaultAsync();
        }

        public async Task<VaccineInventory> GetByIdAsync(int inventoryId)
        {
            return await _dbContext.VaccineInventory
                    .Include(i => i.Vaccine)
                    .Include(i => i.HealthCenter)
                    .FirstOrDefaultAsync(i=>i.VaccineInventoryId == inventoryId);
        }

        public async Task<VaccineInventory> AddStock(VaccineInventory vaccineInventory)
        {
            var inventory = await _dbContext.VaccineInventory.FindAsync(vaccineInventory.VaccineInventoryId);
            if(inventory is null)
            {
                //TODO> Handle it
            }
            inventory.Stock += vaccineInventory.Stock;

            _dbContext.VaccineInventory.Update(inventory);
            await _dbContext.SaveChangesAsync();
            
            return inventory;
        }
    }
}
