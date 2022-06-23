using pry20220181_core_layer.Modules.Inventory.DTOs.Input;
using pry20220181_core_layer.Modules.Inventory.DTOs.Output;
using pry20220181_core_layer.Modules.Inventory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepository _inventoryRepository { get; set; }

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryDTO> GetInventoryByHealthCenterAndVaccine(int healthCenterId, int vaccineId = 0)
        {
            var inventoryFromDb = await _inventoryRepository.GetByHealthCenterAndVaccineAsync(healthCenterId, vaccineId);
            var inventoryToReturn = new InventoryDTO()
            {
                InventoryId = inventoryFromDb.VaccineInventoryId,
                HealthCenterId = inventoryFromDb.HealthCenterId,
                VaccineId = inventoryFromDb.VaccineId,
                VaccineName = inventoryFromDb.Vaccine.Name,
                HealthCenterName = inventoryFromDb.HealthCenter.Name,
                Stock = inventoryFromDb.Stock
            };
            return inventoryToReturn;
        }

        public async Task<InventoryDTO> GetInventoryById(int inventoryId)
        {
            var inventoryFromDb = await _inventoryRepository.GetByIdAsync(inventoryId);
            var inventoryToReturn = new InventoryDTO()
            {
                InventoryId = inventoryFromDb.VaccineInventoryId,
                HealthCenterId = inventoryFromDb.HealthCenterId,
                VaccineId = inventoryFromDb.VaccineId,
                VaccineName = inventoryFromDb.Vaccine.Name,
                HealthCenterName = inventoryFromDb.HealthCenter.Name,
                Stock = inventoryFromDb.Stock
            };
            return inventoryToReturn;
        }

        public async Task<List<InventoryDTO>> GetInventoriesByHealthCenter(int healthCenterId)
        {
            var inventoriesFromDb = await _inventoryRepository.GetAllByHealthCenterAsync(healthCenterId);
            List<InventoryDTO> inventoriesToReturn = new List<InventoryDTO>();
            foreach (var inventory in inventoriesFromDb)
            {
                var inventoryToReturn = new InventoryDTO()
                {
                    InventoryId = inventory.VaccineInventoryId,
                    HealthCenterId = inventory.HealthCenterId,
                    VaccineId = inventory.VaccineId,
                    VaccineName = inventory.Vaccine.Name,
                    HealthCenterName = inventory.HealthCenter.Name,
                    Stock = inventory.Stock
                };
                inventoriesToReturn.Add(inventoryToReturn);
            }
            return inventoriesToReturn;
        }

        public async Task<InventoryDTO> AddVaccineStock(AddVaccineStockDTO inventoryUpdateDTO)
        {
            var updatedInventory = await _inventoryRepository.AddStock(new Models.VaccineInventory()
            {
                VaccineInventoryId = inventoryUpdateDTO.InventoryId,
                Stock = inventoryUpdateDTO.StockToAdd
            });
            return new InventoryDTO()
            {
                InventoryId = updatedInventory.VaccineInventoryId,
                HealthCenterId = updatedInventory.HealthCenterId,
                VaccineId = updatedInventory.VaccineId,
                Stock = updatedInventory.Stock
            };
        }
    }
}
