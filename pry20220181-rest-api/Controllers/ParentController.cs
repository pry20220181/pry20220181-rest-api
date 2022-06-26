using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.Services;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("parents")]
    public class ParentController
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }


        [HttpGet("{parentId}/children", Name = "GetChildren")]
        public async Task<IResult> GetChildren([FromRoute] int parentId)
        {
            //TODO: obtener el ID del usuario conectado, no del path
            var children = await _parentService.GetChildrenAsync(parentId);
            return Results.Ok(new
            {
                Children = children
            });
        }
    }
}
