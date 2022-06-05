using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.DTOs;
using pry20220181_core_layer.Modules.Vaccination.Services;

namespace pry20220181_rest_api.Controllers
{
    [ApiController]
    [Route("vaccines")]
    public class VaccinesController : ControllerBase
    {
        private IVaccineService _vaccineService { get; set; }
        private readonly ILogger<VaccinesController> _logger;

        public VaccinesController(IVaccineService vaccineService, ILogger<VaccinesController> logger)
        {
            _vaccineService = vaccineService;
            _logger = logger;
        }

        [HttpGet(Name = "GetVaccines")]
        public async Task<IEnumerable<VaccineDTO>> Get()
        {
            return await _vaccineService.GetVaccines();
        }

        [HttpGet("{id}", Name = "GetVaccineById")]
        public async Task<VaccineDTO> GetById(int id)
        {
            var vaccine = await _vaccineService.GetVaccineById(id);
            return vaccine;
        }
    }
}
