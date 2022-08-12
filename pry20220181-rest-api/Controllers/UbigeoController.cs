using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Route("ubigeos")]
    [ApiController]
    public class UbigeoController : ControllerBase
    {
        private IUbigeoService _ubigeoService { get; set; }
        ILogger<UbigeoController> _logger;

        public UbigeoController(ILogger<UbigeoController> logger, IUbigeoService ubigeoService)
        {
            _logger = logger;
            _ubigeoService = ubigeoService;
        }

        [HttpGet(Name = "GetUbigeos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Ubigeos", typeof(List<UbigeoDTO>))]
        public async Task<IResult> GetUbigeos()
        {
            try
            {
                var ubigeos = await _ubigeoService.GetUbigeosAsync();

                return Results.Ok(new {
                    Ubigeos = ubigeos
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
