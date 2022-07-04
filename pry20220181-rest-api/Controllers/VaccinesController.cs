using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Services;
using pry20220181_core_layer.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Vaccines", typeof(List<VaccineDTO>))]
        public async Task<IResult> Get([FromQuery] PaginationParameter paginationParameter, [FromQuery] string? fields = GetVaccinesMode.WithAllInfo)
        {
            try
            {
                var vaccines = await _vaccineService.GetVaccinesCompleteInfoAsync(paginationParameter, fields);
                return Results.Ok(new
                {
                    Vaccines = vaccines
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode : 500);
            }
        }

        [HttpGet("{id}", Name = "GetVaccineById")]
        public async Task<VaccineDTO> GetById(int id)
        {
            //var vaccine = await _vaccineService.GetVaccineByIdAsync(id);
            var vaccine = await _vaccineService.GetVaccineCompleteInfoByIdAsync(id);
            return vaccine;
        }

        [HttpPost(Name = "CreateVaccine")]
        public async Task<int> Create(VaccineCreationDTO vaccineCreationDTO)
        {
            return await _vaccineService.CreateVaccineAsync(vaccineCreationDTO);
        }

        [HttpPut("{id}", Name = "UpdateVaccine")]
        public async Task<VaccineDTO> Update(int id, VaccineUpdateDTO vaccineUpdateDTO)
        {
            return await _vaccineService.UpdateVaccineAsync(id, vaccineUpdateDTO);
        }

        [HttpDelete("{id}", Name = "DeleteVaccine")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _vaccineService.DeleteVaccineAsync(id);
            return response ? Ok() : NotFound();
        }
    }
}
