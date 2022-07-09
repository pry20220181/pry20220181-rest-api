using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Campaigns.DTOs.Input;
using pry20220181_core_layer.Modules.Campaigns.DTOs.Output;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Campaigns.Services;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route(template: "vaccination/campaigns")]
    public class VaccinationCampaignController : Controller
    {
        private IVaccinationCampaignsService _vaccinationCampaignsService { get; set; }
        private readonly ILogger<VaccinesController> _logger;

        public VaccinationCampaignController(IVaccinationCampaignsService vaccinationCampaignsService, ILogger<VaccinesController> logger)
        {
            _vaccinationCampaignsService = vaccinationCampaignsService;
            _logger = logger;
        }

        [HttpGet(Name = "GetCampaignsByHealthCenter")]
        public async Task<IResult> GetByHealthCenterId([FromQuery] int healthCenterId = 0)
        {
            if (healthCenterId == 0)
            {
                return Results.BadRequest("HealthCenterId is required");
            }
            var vaccinationCampaigns = await _vaccinationCampaignsService.GetVaccinationCampaignsByHealthCenter(healthCenterId);
            return Results.Ok(new
            {
                VaccinationCampaigns = vaccinationCampaigns
            });
        }

        [HttpGet("{campaignId}", Name = "GetCampaignById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Vaccination Campaign", typeof(VaccinationCampaignDetailDTO))]
        public async Task<IResult> GetById([FromRoute] int campaignId)
        {
            try
            {
                var vaccinationCampaign = await _vaccinationCampaignsService.GetVaccinationCampaignById(campaignId);

                if (vaccinationCampaign is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new
                {
                    VaccinationCampaign = vaccinationCampaign
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost(Name = "CreateVaccinationCampaign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Create Vaccination Campaign", typeof(int))]
        public async Task<IResult> CreateVaccinationCampaign([FromBody] VaccinationCampaignCreateDTO vaccinationCampaignCreateDTO)
        {
            try
            {
                if (vaccinationCampaignCreateDTO is null)
                {
                    return Results.BadRequest("Not information to register is found");
                }

                var createdCampaignId = await _vaccinationCampaignsService.CreateVaccinationCampaign(vaccinationCampaignCreateDTO);

                if (createdCampaignId < 1)
                {
                    var errorMessage = "Something was wrong when create the vaccination campaign";
                    _logger.LogError(errorMessage);
                    return Results.BadRequest(errorMessage);
                }

                return Results.Ok(new
                {
                    VaccinationCampaignId = createdCampaignId
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
