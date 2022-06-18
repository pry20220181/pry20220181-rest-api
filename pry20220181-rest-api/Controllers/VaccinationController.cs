﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
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

        [HttpGet("{childId}/remaining-doses", Name = "GetChildsRemainingDoses")]
        public async Task<List<RemainingDoseDTO>> GetChildsRemainingDoses(int childId)
        {
            var remainingDoses = await _dosesService.GetRemainingDosesByChild(childId);
            return remainingDoses;
        }

        [HttpPost("{childId}/doses", Name = "RegisterAdministeredDose")]
        public async Task<int> RegisterAdministeredDose(int childId, [FromBody] AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            var registeredAdministeredDoseId = await _dosesService.CreateAdministeredDose(administeredDoseCreationDTO);
            return registeredAdministeredDoseId;
        }
    }
}
