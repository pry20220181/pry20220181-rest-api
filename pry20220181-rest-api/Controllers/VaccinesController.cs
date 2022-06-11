using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Services;
using Sieve.Models;
using Sieve.Services;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    //[ApiController]
    [Route("vaccines")]
    public class VaccinesController : ControllerBase
    {
        private IVaccineService _vaccineService { get; set; }
        private SieveProcessor _sieveProcessor { get; set; }
        private readonly ILogger<VaccinesController> _logger;

        public VaccinesController(IVaccineService vaccineService, ILogger<VaccinesController> logger, SieveProcessor sieveProcessor)
        {
            _vaccineService = vaccineService;
            _logger = logger;
            _sieveProcessor = sieveProcessor;
        }

        //[HttpGet(Name = "GetVaccines")]
        //public async Task<IEnumerable<VaccineDTO>> Get()
        //{
        //    return await _vaccineService.GetVaccinesAsync();
        //}

        [HttpGet(Name = "GetVaccines")]
        public IEnumerable<VaccineDTO> Get(SieveModel sieveModel)
        {
            var result = _sieveProcessor.Apply(sieveModel, _vaccineService.GetIQueryableVaccines());
            return result.ToList();
        }

        [HttpGet("{id}", Name = "GetVaccineById")]
        public async Task<VaccineDTO> GetById([FromQuery] int id)
        {
            var vaccine = await _vaccineService.GetVaccineByIdAsync(id);
            return vaccine;
        }

        [HttpPost(Name = "CreateVaccine")]
        public async Task<int> Create([FromBody] VaccineCreationDTO vaccineCreationDTO)
        {
            return await _vaccineService.CreateVaccineAsync(vaccineCreationDTO);
        }

        [HttpPut("{id}", Name = "UpdateVaccine")]
        public async Task<VaccineDTO> Update([FromQuery] int id, [FromBody] VaccineUpdateDTO vaccineUpdateDTO)
        {
            return await _vaccineService.UpdateVaccineAsync(id, vaccineUpdateDTO);
        }

        [HttpDelete("{id}", Name = "DeleteVaccine")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _vaccineService.DeleteVaccineAsync(id);
            return response ? Ok() : NotFound();
        }
    }
}
