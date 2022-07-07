using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_rest_api.Security.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pry20220181_rest_api.Controllers
{
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IParentService _parentService;
        private readonly IHealthPersonnelService _healthPersonnelService;
        private readonly JwtSection jwtSection;
        ILogger<SecurityController> _logger;

        public SecurityController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IParentRepository parentRepository, IParentService parentService, IHealthPersonnelService healthPersonnelService, ILogger<SecurityController> logger, JwtSection jwtSection)
        {
            _roleManager = roleManager;

            _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Parent"
            });
            _roleManager.CreateAsync(new IdentityRole
            {
                Name = "HealthPersonnel"
            });

            _userManager = userManager;
            _parentService = parentService;
            _healthPersonnelService = healthPersonnelService;
            _logger = logger;
            this.jwtSection = jwtSection;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(200, "Authenticate", typeof(string))]
        public async Task<IResult> Authenticate(AuthenticateRequest request)
        {
            try
            {
                // Verificamos credenciales con Identity
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    _logger.LogWarning($"The User {request.Username} maybe does not exist or their password is incorrect");
                    return Results.BadRequest(new
                    {
                        StatusCode = 403,
                        Message = "Wrong credentials"
                    });
                }

                var roles = await _userManager.GetRolesAsync(user);

                // Generamos un token según los claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: jwtSection.Issuer,
                    audience: jwtSection.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(jwtSection.ExpirationMinutes),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

                return Results.Ok(new
                {
                    AccessToken = jwt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost("parent", Name = "RegisterParent")]
        public async Task<IResult> RegisterParent(ParentCreateDTO parentCreateDTO)
        {
            var newUser = new User
            {
                Email = parentCreateDTO.Email,
                FirstName = parentCreateDTO.FirstName,
                LastName = parentCreateDTO.LastName,
                UserName = parentCreateDTO.DNI
            };

            await _userManager.CreateAsync(newUser, parentCreateDTO.Password);
            await _userManager.AddToRoleAsync(newUser, "Parent");
            parentCreateDTO.UserId = newUser.Id;

            var newParentId = await _parentService.RegisterParentAndChildrenAsync(parentCreateDTO);
            return Results.Ok(newParentId);
        }

        [HttpGet("parent/{parentId}", Name = "GetParentInfo")]
        public async Task<IResult> GetParent(int parentId)
        {
            return Results.Ok(new
            {
                Parent = await _parentService.GetParentAsync(parentId)
            });
        }

        [HttpPost("health-personnel", Name = "RegisterHealthPersonnel")]
        public async Task<IResult> RegisterHealthPersonnel(HealthPersonnelCreateDTO healthPersonnelCreateDTO)
        {
            var newUser = new User
            {
                Email = healthPersonnelCreateDTO.Email,
                FirstName = healthPersonnelCreateDTO.FirstName,
                LastName = healthPersonnelCreateDTO.LastName,
                UserName = healthPersonnelCreateDTO.DNI
            };

            await _userManager.CreateAsync(newUser, healthPersonnelCreateDTO.Password);
            await _userManager.AddToRoleAsync(newUser, "HealthPersonnel");
            healthPersonnelCreateDTO.UserId = newUser.Id;

            var nnewHealthPersonelId = await _healthPersonnelService.RegisterHealthPersonnelAsync(healthPersonnelCreateDTO);
            return Results.Ok(nnewHealthPersonelId);
        }

        [HttpGet("health-personnel/{healthPersonnelId}", Name = "GetHealthPersonnelInfo")]
        public async Task<IResult> GetHealthPersonnel(int healthPersonnelId)
        {
            return Results.Ok(new
            {
                HealthPersonnel = await _healthPersonnelService.GetHealthPersonnelAsync(healthPersonnelId)
            });
        }

        [HttpGet("register")]
        public async Task<IResult> Register()
        {
            if (!_userManager.Users.Any())
            {
                var newUser = new User
                {
                    Email = "test@demo.com",
                    FirstName = "Test",
                    LastName = "User",
                    UserName = "test.demo"
                };

                await _userManager.CreateAsync(newUser, "P@ss.W0rd");

                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "AnotherRole"
                });
                await _userManager.AddToRoleAsync(newUser, "Admin");
                await _userManager.AddToRoleAsync(newUser, "AnotherRole");
                return Results.Ok(new
                {
                    Response = "Default user created"
                });
            }
            else
            {
                return Results.Ok(new
                {
                    Response = "Default user already exists"
                });
            }
        }
    }
}