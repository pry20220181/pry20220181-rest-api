using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Services;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("vaccination")]
    public class VaccinationController : ControllerBase
    {
        private readonly IDosesService _dosesService;

        public VaccinationController(IDosesService dosesService)
        {
            _dosesService = dosesService;
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

        [HttpPost("doses", Name = "RegisterAdministeredDose")]
        public async Task<int> RegisterAdministeredDose([FromBody] AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            var registeredAdministeredDoseId = await _dosesService.CreateAdministeredDose(administeredDoseCreationDTO);
            return registeredAdministeredDoseId;
        }

        [HttpGet("doses", Name = "GetChildsAdministeredDoses")]
        public async Task<IResult> GetChildsAdministeredDoses([FromQuery] int childId = 0)
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
    }
}
