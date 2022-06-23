using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Inventory.DTOs.Input;
using pry20220181_core_layer.Modules.Inventory.Services;

namespace pry20220181_rest_api.Controllers
{
    [ApiController]
    [Route("vaccines/inventory")]
    public class InventoryController
    {
        private IInventoryService _inventoryService { get; set; }

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet(Name = "GetInventoryByHealthCenter")]
        public async Task<IResult> GetInventoryByHealthCenter([FromQuery] int healthCenterId = 0, [FromQuery] int vaccineId = 0)
        {
            if (healthCenterId == 0)
            {
                return Results.BadRequest("healthCenterId is required");
            }
            if (vaccineId > 0)
            {
                var inventoryToReturn = await _inventoryService.GetInventoryByHealthCenterAndVaccine(healthCenterId, vaccineId);
                return Results.Ok(new
                {
                    Inventory = inventoryToReturn
                });
            }
            var inventoriesToReturn = await _inventoryService.GetInventoriesByHealthCenter(healthCenterId);
            return Results.Ok(new
            {
                Inventories = inventoriesToReturn
            });
        }

        [HttpGet("{inventoryId}", Name = "GetInventoryById")]
        public async Task<IResult> GetInventoryById([FromRoute] int inventoryId = 0)
        {
            if (inventoryId == 0)
            {
                return Results.BadRequest("inventoryId is required");
            }
            var inventoryToReturn = await _inventoryService.GetInventoryById(inventoryId);
            return Results.Ok(new
            {
                Inventory = inventoryToReturn
            });
        }

        [HttpPost(Name = "AddVaccineStock")]
        public async Task<IResult> AddVaccineStock([FromBody]AddVaccineStockDTO inventoryUpdateDTO)
        {
            var updatedInventory = await _inventoryService.AddVaccineStock(inventoryUpdateDTO);
            return Results.Ok(new
            {
                Inventory = updatedInventory
            });
        }
    }
}