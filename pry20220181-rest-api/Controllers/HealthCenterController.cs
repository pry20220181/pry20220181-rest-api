using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Route("health-centers")]
    [ApiController]
    public class HealthCenterController : ControllerBase
    {
        private IHealthCenterService _healthCenterService { get; set; }
        ILogger<HealthCenterController> _logger;

        public HealthCenterController(ILogger<HealthCenterController> logger, IHealthCenterService healthCenterService)
        {
            _logger = logger;
            _healthCenterService = healthCenterService;
        }

        [HttpGet(Name = "GetHealthCentersByUbigeos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "GetHealthCentersByUbigeos", typeof(List<HealthCenterDTO>))]
        public async Task<IResult> GetChildsRemainingDoses([FromQuery] string ubigeoIds = "")
        {
            try
            {
                if (ubigeoIds == "")
                {
                    return Results.BadRequest("UbigeoIds is required");
                }
                List<int> ubigeoIdsList = new List<int>();
                try
                {
                    ubigeoIdsList =  ubigeoIds.Split(',').Select(c => Convert.ToInt32(c.Trim())).ToList();
                }
                catch (Exception)
                {
                    return Results.BadRequest("UbigeoIds has not the right format (numbers in csv: 1,2,3)");
                }

                var healthCenters = await _healthCenterService.GetHealthCentersByUbigeosAsync(ubigeoIdsList);

                return Results.Ok(new
                {
                    HealthCenters = healthCenters
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
