using pry20220181_core_layer.Modules.Inventory.DTOs.Input;
using pry20220181_core_layer.Modules.Inventory.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.Services
{
    public interface IInventoryService
    {
        public Task<List<InventoryDTO>> GetInventoriesByHealthCenter(int healthCenterId);
        public Task<InventoryDTO> GetInventoryById(int inventoryId);
        public Task<InventoryDTO> GetInventoryByHealthCenterAndVaccine(int healthCenterId, int vaccineId = 0);
        /// <summary>
        /// Add units to the current stock of the specified inventory
        /// </summary>
        /// <param name="inventoryUpdateDTO"></param>
        /// <returns>The Updated Inventory with the new stock</returns>
        public Task<InventoryDTO> AddVaccineStock(AddVaccineStockDTO inventoryUpdateDTO);
    }
}
