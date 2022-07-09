using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Inventory.DTOs.Input;
using pry20220181_core_layer.Modules.Inventory.DTOs.Output;
using pry20220181_core_layer.Modules.Inventory.Services;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [ApiController]
    [Route("vaccines/inventory")]
    public class InventoryController
    {
        private IInventoryService _inventoryService { get; set; }
        private ILogger<InventoryController> _logger { get; set; }

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet(Name = "GetInventoryByHealthCenter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Inventory By HealthCenter and Vaccine", typeof(InventoryDTO))]
        [SwaggerResponse(200, "Get Inventories By HealthCenter", typeof(List<InventoryDTO>))]
        public async Task<IResult> GetInventoryByHealthCenter([FromQuery] int healthCenterId = 0, [FromQuery] int vaccineId = 0)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
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