using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("parents")]
    public class ParentController
    {
        private readonly IParentService _parentService;
        private ILogger<ParentController> _logger { get; set; }

        public ParentController(IParentService parentService, ILogger<ParentController> logger)
        {
            _parentService = parentService;
            _logger = logger;
        }


        [HttpGet("{parentId}/children", Name = "GetChildren")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Get Children by Parent", typeof(List<ChildDTO>))]
        public async Task<IResult> GetChildren([FromRoute] int parentId = 0)
        {
            try
            {
                if(parentId == 0)
                {
                    return Results.BadRequest();
                }

                //TODO: obtener el ID del usuario conectado, no del path
                var children = await _parentService.GetChildrenAsync(parentId);
                return Results.Ok(new
                {
                    Children = children
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
