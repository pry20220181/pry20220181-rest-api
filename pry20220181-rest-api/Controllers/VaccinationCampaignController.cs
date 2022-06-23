using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Campaigns.DTOs.Input;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Campaigns.Services;
using pry20220181_core_layer.Modules.Vaccination.Services;

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
        public async Task<IResult> GetById([FromRoute] int campaignId)
        {
            var vaccinationCampaign = await _vaccinationCampaignsService.GetVaccinationCampaignById(campaignId);
            return Results.Ok(new
            {
                VaccinationCampaign = vaccinationCampaign
            });
        }

        [HttpPost(Name = "CreateVaccinationCampaign")]
        public async Task<IResult> CreateVaccinationCampaign([FromBody] VaccinationCampaignCreateDTO vaccinationCampaignCreateDTO)
        {
            var createdCampaignId = await _vaccinationCampaignsService.CreateVaccinationCampaign(vaccinationCampaignCreateDTO);
            return Results.Ok(createdCampaignId);
        }
    }
}
