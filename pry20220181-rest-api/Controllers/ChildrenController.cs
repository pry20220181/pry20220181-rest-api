using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_rest_api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("children")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;
        private ILogger<ChildrenController> _logger { get; set; }

        public ChildrenController(IChildService childService, ILogger<ChildrenController> logger)
        {
            _childService = childService;
            _logger = logger;
        }

        [HttpGet(Name = "GetChildren")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Children by DNI", typeof(ChildDTO))]
        [SwaggerResponse(200, "Get Children", typeof(List<ChildDTO>))]
        public async Task<IResult> GetChildren([FromQuery] string? dni = "")
        {
            try
            {
                if (dni == "")
                {
                    var children = await _childService.GetChildren();

                    return Results.Ok(new
                    {
                        Children = children
                    });
                }
                else
                {
                    var child = await _childService.GetChildByDniAsync(dni);

                    if (child is null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(new
                    {
                        Child = child
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }

        }


        [HttpGet("{childId}/vaccination-card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Child's Vaccination Card", typeof(VaccinationCardDTO))]
        public async Task<IResult> GetVaccinationCard([FromRoute] int childId = 0)
        {
            try
            {
                if (childId == 0)
                {
                    return Results.BadRequest("ChildId is required");
                }
                
                //TRAER LOS CHILDREN DEL PARENT Y VER SI LO INCLUYEK SINO RETORNO 401 o 403

                var user = HttpContext.User;
                var parentId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);

                var vaccinationCard = await _childService.GetVaccinationCardAsync(childId, parentId);
                if (vaccinationCard is null)
                {
                    var errorMessage = $"Something was wrong when get the vaccination card of child with ID {childId}";
                    _logger.LogError(errorMessage);
                    return Results.BadRequest(errorMessage);
                }

                return Results.Ok(new
                {
                    VaccinationCard = vaccinationCard
                });
            }
            catch(NotIsTheirChildException ex){
                _logger.LogWarning(ex.Message);
                return Results.BadRequest(new
                {
                    StatusCode = 403,
                    Message = "No access"
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
