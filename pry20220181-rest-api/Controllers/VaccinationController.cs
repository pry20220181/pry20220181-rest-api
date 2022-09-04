using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Services;
using pry20220181_rest_api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using static pry20220181_core_layer.Modules.Master.DTOs.Output.VaccinationCardDTO;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("vaccination")]
    public class VaccinationController : ControllerBase
    {
        private readonly IDosesService _dosesService;
        private readonly IVaccinationSchemeService _vaccinationSchemeService;
        ILogger<VaccinationController> _logger;

        public VaccinationController(IDosesService dosesService, ILogger<VaccinationController> logger, IVaccinationSchemeService vaccinationSchemeService)
        {
            _dosesService = dosesService;
            _logger = logger;
            _vaccinationSchemeService = vaccinationSchemeService;
        }

        [HttpGet("schemes", Name = "GetVaccinationSchemes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Vaccination Schemes", typeof(List<VaccinationSchemeDTO>))]
        public async Task<IResult> GetVaccinationSchemes()
{
            try
            {
                var vaccinationSchemes = await _vaccinationSchemeService.GetAllVaccinationSchemes();

                return Results.Ok(new
                {
                    VaccinationSchemes = vaccinationSchemes
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpGet("remaining-doses", Name = "GetChildsRemainingDoses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Child's Remaining Doses", typeof(List<RemainingDoseDTO>))]
        public async Task<IResult> GetChildsRemainingDoses([FromQuery] int childId = 0)
        {
            try
            {
                if (childId == 0)
                {
                    return Results.BadRequest("ChildId is required");
                }
                var remainingDoses = await _dosesService.GetRemainingDosesByChild(childId);

                return Results.Ok(new
                {
                    RemainingDoses = remainingDoses
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost("administered-doses", Name = "RegisterAdministeredDose")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Register an Administered Dose", typeof(List<AdministeredDoseDTO>))]
        public async Task<IResult> RegisterAdministeredDose([FromBody] AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            try
            {
                var user = HttpContext.User;
                var healthPersonnelId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);
                if (healthPersonnelId == 0)
                {
                    return Results.BadRequest();
                }

                administeredDoseCreationDTO.HealthPersonnelId = healthPersonnelId;

                var registeredAdministeredDoseId = await _dosesService.CreateAdministeredDose(administeredDoseCreationDTO);
                return Results.Ok(new
                {
                    AdministeredDoseId = registeredAdministeredDoseId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
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

        [HttpGet("administered-doses/{id}", Name = "GetAdministeredDoseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Administered Dose By Id", typeof(AdministeredDoseDTO))]
        public async Task<IResult> GetAdministeredDoseById([FromRoute] int id = 0)
        {
            try
            {
                return Results.Ok(new
                {
                    AdministeredDose = new AdministeredDoseDTO(){
                        AdministeredDoseId = id.ToString()
                    }
                });

                // var administeredDose = await _dosesService.GetAdministeredDoseById(id);
                // return Results.Ok(new
                // {
                //     AdministeredDose = administeredDose
                // });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }
    }
}
