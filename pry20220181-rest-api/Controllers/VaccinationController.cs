using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("vaccination")]
    public class VaccinationController : ControllerBase
    {
        private readonly IDosesService _dosesService;
        ILogger<VaccinationController> _logger;

        public VaccinationController(IDosesService dosesService, ILogger<VaccinationController> logger)
        {
            _dosesService = dosesService;
            _logger = logger;
        }

        [HttpGet("remaining-doses", Name = "GetChildsRemainingDoses")]
        public async Task<List<RemainingDoseDTO>> GetChildsRemainingDoses([FromQuery] int childId = 0)
        {
            if (childId == 0)
            {
                return null;
            }
            var remainingDoses = await _dosesService.GetRemainingDosesByChild(childId);
            return remainingDoses;
        }

        [HttpPost("administered-doses", Name = "RegisterAdministeredDose")]
        public async Task<int> RegisterAdministeredDose([FromBody] AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            var registeredAdministeredDoseId = await _dosesService.CreateAdministeredDose(administeredDoseCreationDTO);
            return registeredAdministeredDoseId;
        }

        [HttpGet("administered-doses", Name = "GetChildsAdministeredDoses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Child's Administered Doses", typeof(List<AdministeredDoseDTO>))]
        public async Task<IResult> GetChildsAdministeredDoses([FromQuery] int childId = 0)
        {
            try
            {
                if (childId == 0)
                {
                    return Results.BadRequest("ChildId is required");
                }

                var administeredDoses = await _dosesService.GetAdministeredDosesByChild(childId);
                return Results.Ok(new
                {
                    AdministeredDoses = administeredDoses
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }
    }
}
