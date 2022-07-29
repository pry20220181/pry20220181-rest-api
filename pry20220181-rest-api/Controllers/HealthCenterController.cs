using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Route("health-center")]
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
        [SwaggerResponse(200, "GetHealthCentersByUbigeos", typeof(List<RemainingDoseDTO>))]
        public async Task<IResult> GetChildsRemainingDoses([FromQuery] string ubigeoIds = "")
        {
            try
            {
                if (childId == 0)
                {
                    return Results.BadRequest("ChildId is required");
                }
                var remainingDoses = await _dosesService.GetRemainingDosesByChild(childId);

                return Results.Ok(new
                {
                    RemainingDoses = remainingDoses
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
