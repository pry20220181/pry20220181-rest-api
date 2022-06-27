using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.Services;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("vaccination/appointments")]
    public class VaccinationAppointmentController : ControllerBase
    {
        private readonly IVaccinationAppointmentService _vaccinationAppointmentService;

        public VaccinationAppointmentController(IVaccinationAppointmentService vaccinationAppointmentService)
        {
            _vaccinationAppointmentService = vaccinationAppointmentService;
        }

        [HttpPost(Name = "RegisterVaccinationAppointment")]
        public async Task<IResult> RegisterVaccinationAppointment([FromBody] VaccinationAppointmentCreationDTO vaccinationAppointmentCreationDTO)
        {
            var registeredVaccinationAppointmentId = await _vaccinationAppointmentService.CreateVaccinationAppointmentAsync(vaccinationAppointmentCreationDTO);
            return Results.Ok(new
            {
                VaccinationAppointmentId = registeredVaccinationAppointmentId
            });
        }
    }
}
