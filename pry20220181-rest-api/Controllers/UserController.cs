using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Utils;
using pry20220181_rest_api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpGet("info", Name = "GetUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "GetUserInfo", typeof(UserInfoDTO))]
        public async Task<IResult> GetUserInfo()
        {
            var user = HttpContext.User;
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            var dni = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.DNI).Value;

            var firstname = "";
            var lastName = "";
            var email = "";
            if (role == Roles.HealthPersonnel)
            {
                var healthPersonnel = await _userService.GetHealthPersonnelByUserIdAsync(userId);
                firstname = healthPersonnel.FirstName;
                lastName = healthPersonnel.LastName;
                email = healthPersonnel.Email;
            }
            else if (role == Roles.Parent)
            {
                var parent = await _userService.GetParentByUserIdAsync(userId);
                firstname = parent.FirstName;
                lastName = parent.LastName;
                email = parent.Email;
            }
            else
            {
                return Results.Ok("Something was wrong");
            }

            return Results.Ok(new
            {
                UserInfo = new UserInfoDTO()
                {
                    DNI = dni,
                    FirstName = firstname,
                    LastName = lastName,
                    Email = email
                }
            });
        }
    }
}