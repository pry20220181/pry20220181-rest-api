using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("children")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;
        private ILogger<ChildrenController> _logger { get; set; }

        public ChildrenController(IChildService childService, ILogger<ChildrenController> logger)
        {
            _childService = childService;
            _logger = logger;
        }

        [HttpGet(Name = "GetChildByDni")]
        public async Task<ChildDTO> GetByDni([FromQuery] string dni)
        {
            var child = await _childService.GetChildByDniAsync(dni);
            return child;
        }


        [HttpGet("{childId}/vaccination-card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Child's Vaccination Card", typeof(VaccinationCardDTO))]
        public async Task<IResult> GetVaccinationCard([FromRoute] int childId = 0)
        {
            if(childId == 0)
            {
                return Results.BadRequest("ChildId is required");
            }
            var vaccinationCard = await _childService.GetVaccinationCardAsync(childId);
            if(vaccinationCard is null)
            {
                var errorMessage = $"Something was wrong when get the vaccination card of child with ID {childId}";
                _logger.LogError(errorMessage);
                return Results.BadRequest(errorMessage);
            }
            return Results.Ok(new
            {
                VaccinationCard = vaccinationCard
            });
        }
    }
}
