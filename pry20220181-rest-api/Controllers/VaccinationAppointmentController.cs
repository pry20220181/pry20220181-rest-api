using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.Services;
using pry20220181_core_layer.Modules.Vaccination.Services.Impl;
using pry20220181_core_layer.Utils;
using pry20220181_rest_api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Authorize(Roles = Roles.Parent)]
    [ApiController]
    [Route("vaccination/appointments")]
    public class VaccinationAppointmentController : ControllerBase
    {
        private readonly IVaccinationAppointmentService _vaccinationAppointmentService;
        private ILogger<VaccinationAppointmentController> _logger { get; set; }

        public VaccinationAppointmentController(IVaccinationAppointmentService vaccinationAppointmentService, ILogger<VaccinationAppointmentController> logger)
        {
            _vaccinationAppointmentService = vaccinationAppointmentService;
            _logger = logger;
        }

        [HttpPost(Name = "RegisterVaccinationAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Register Vaccination Appointment", typeof(int))]
        public async Task<IResult> RegisterVaccinationAppointment([FromBody] VaccinationAppointmentCreationDTO vaccinationAppointmentCreationDTO)
        {
            try
            {
                if (vaccinationAppointmentCreationDTO is null)
                {
                    return Results.BadRequest("Not information to register is found");
                }

                var user = HttpContext.User;
                var parentId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);
                if (parentId == 0)
                {
                    return Results.BadRequest();
                }

                vaccinationAppointmentCreationDTO.ParentId = parentId;

                var registeredVaccinationAppointmentId = await _vaccinationAppointmentService.CreateVaccinationAppointmentAsync(vaccinationAppointmentCreationDTO);

                if (registeredVaccinationAppointmentId < 1)
                {
                    var errorMessage = "Something was wrong when create the vaccination apointment";
                    _logger.LogError(errorMessage);
                    return Results.BadRequest(errorMessage);
                }

                return Results.Ok(new
                {
                    VaccinationAppointmentId = registeredVaccinationAppointmentId
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
