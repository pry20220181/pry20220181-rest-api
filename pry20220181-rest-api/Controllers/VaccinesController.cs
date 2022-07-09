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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "Vaccine", typeof(VaccineDTO))]
        public async Task<IResult> GetById(int id)
        {
            try
            {
                var vaccine = await _vaccineService.GetVaccineCompleteInfoByIdAsync(id);

                if(vaccine is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new
                {
                    Vaccine = vaccine
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost(Name = "CreateVaccine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Create Vaccine", typeof(int))]
        public async Task<IResult> Create(VaccineCreationDTO vaccineCreationDTO)
        {
            try
            {
                var createdVaccineId = await _vaccineService.CreateVaccineAsync(vaccineCreationDTO);
                return Results.Ok(new
                {
                    VaccineId = createdVaccineId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
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
