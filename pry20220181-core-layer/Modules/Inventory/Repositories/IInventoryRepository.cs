using pry20220181_core_layer.Modules.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.Repositories
{
    public interface IInventoryRepository
    {
        public Task<List<VaccineInventory>> GetAllByHealthCenterAsync(int healthCenterId);
        public Task<VaccineInventory> GetByIdAsync(int inventoryId);
        public Task<VaccineInventory> GetByHealthCenterAndVaccineAsync(int healthCenterId, int vaccineId);
    }
}
