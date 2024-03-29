using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Utils;
using pry20220181_rest_api.Security.Models;
using pry20220181_rest_api.Utils;
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
        private readonly IUserService _userService;
        private readonly IParentService _parentService;
        private readonly IHealthPersonnelService _healthPersonnelService;
        private readonly JwtSection jwtSection;
        ILogger<SecurityController> _logger;

        public SecurityController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IParentRepository parentRepository, IParentService parentService, IHealthPersonnelService healthPersonnelService, ILogger<SecurityController> logger, JwtSection jwtSection, IUserService userService)
        {
            _roleManager = roleManager;

            _roleManager.CreateAsync(new IdentityRole
            {
                Name = Roles.Parent
            });
            _roleManager.CreateAsync(new IdentityRole
            {
                Name = Roles.HealthPersonnel
            });

            _userManager = userManager;
            _parentService = parentService;
            _healthPersonnelService = healthPersonnelService;
            _logger = logger;
            this.jwtSection = jwtSection;
            _userService = userService;
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
                bool entityInfoObtained = false;
                string roleToReturn = "";
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    if (!entityInfoObtained)
                    {
                        int entityId = 0;
                        string entityDNI = "";
                        if (role == Roles.Parent)
                        {
                            var result = await _userService.GetParentByUserIdAsync(user.Id);
                            entityId = result.ParentId;
                            entityDNI = result.DNI;
                            roleToReturn = Roles.Parent;
                        }
                        else if (role == Roles.HealthPersonnel)
                        {
                            var result = await _userService.GetHealthPersonnelByUserIdAsync(user.Id);
                            entityId = result.HealthPersonnelId;
                            entityDNI = result.DNI;
                            roleToReturn = Roles.HealthPersonnel;
                        }
                        else
                        {
                            return Results.Problem();
                        }
                        claims.Add(new Claim(CustomClaimTypes.EntityId, entityId.ToString()));
                        claims.Add(new Claim(CustomClaimTypes.DNI, entityDNI));
                        entityInfoObtained = true;
                    }
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
                    AccessToken = jwt,
                    Role = roleToReturn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost("parent", Name = "RegisterParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "RegisterParent", typeof(int))]
        public async Task<IResult> RegisterParent(ParentCreateDTO parentCreateDTO)
        {
            try
            {
                var newUser = new User
                {
                    Email = parentCreateDTO.Email,
                    FirstName = parentCreateDTO.FirstName,
                    LastName = parentCreateDTO.LastName,
                    UserName = parentCreateDTO.DNI
                };

                await _userManager.CreateAsync(newUser, parentCreateDTO.Password);
                await _userManager.AddToRoleAsync(newUser, Roles.Parent);

                parentCreateDTO.UserId = newUser.Id;

                var newParentId = await _parentService.RegisterParentAndChildrenAsync(parentCreateDTO);

                if (newParentId < 1)
                {
                    var errorMessage = $"Something was wrong when create the parent {parentCreateDTO.Email}";
                    _logger.LogError(errorMessage);
                    return Results.BadRequest(errorMessage);
                }

                return Results.Ok(new
                {
                    ParentId = newParentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost("register-parents", Name = "RegisterParents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "RegisterParents", typeof(List<RegisteredParentDTO>))]
        public async Task<IResult> RegisterParents(List<ParentCreateDTO> parentsCreateDTO)
        {
            _logger.LogInformation("Register and authenticate Parents process started");
            List<RegisteredParentDTO> registeredParents = new List<RegisteredParentDTO>();
            foreach (var parentCreateDTO in parentsCreateDTO)
            {
                try
                {
                    #region Register User
                    var newUser = new User
                    {
                        Email = parentCreateDTO.Email,
                        FirstName = parentCreateDTO.FirstName,
                        LastName = parentCreateDTO.LastName,
                        UserName = parentCreateDTO.DNI
                    };

                    await _userManager.CreateAsync(newUser, parentCreateDTO.Password);
                    await _userManager.AddToRoleAsync(newUser, Roles.Parent);

                    parentCreateDTO.UserId = newUser.Id;

                    var newParentToReturn = await _parentService.RegisterParentAndChildrenForValidationAsync(parentCreateDTO);

                    if (newParentToReturn is null)
                    {
                        var errorMessage = $"Something was wrong when create the parent {parentCreateDTO.Email}";
                        _logger.LogError(errorMessage);
                        continue;
                    }

                    newParentToReturn.Username = parentCreateDTO.DNI;
                    newParentToReturn.UserId = newUser.Id;

                    _logger.LogInformation($"User {parentCreateDTO.DNI} created");
                    #endregion

                    #region Authenticate Users
                    // Verificamos credenciales con Identity
                    var user = await _userManager.FindByNameAsync(parentCreateDTO.DNI);

                    if (user is null || !await _userManager.CheckPasswordAsync(user, parentCreateDTO.Password))
                    {
                        _logger.LogWarning($"The User {parentCreateDTO.DNI} maybe does not exist or their password is incorrect (Error 403 - Wrong credentials)");
                    }

                    var roles = await _userManager.GetRolesAsync(user);

                    // Generamos un token según los claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                    };
                    bool entityInfoObtained = false;
                    string roleToReturn = "";
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        if (!entityInfoObtained)
                        {
                            int entityId = 0;
                            string entityDNI = "";
                            if (role == Roles.Parent)
                            {
                                var result = await _userService.GetParentByUserIdAsync(user.Id);
                                entityId = result.ParentId;
                                entityDNI = result.DNI;
                                roleToReturn = Roles.Parent;
                            }
                            else if (role == Roles.HealthPersonnel)
                            {
                                var result = await _userService.GetHealthPersonnelByUserIdAsync(user.Id);
                                entityId = result.HealthPersonnelId;
                                entityDNI = result.DNI;
                                roleToReturn = Roles.HealthPersonnel;
                            }
                            else
                            {
                                return Results.Problem();
                            }
                            claims.Add(new Claim(CustomClaimTypes.EntityId, entityId.ToString()));
                            claims.Add(new Claim(CustomClaimTypes.DNI, entityDNI));
                            entityInfoObtained = true;
                        }
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
                    newParentToReturn.Jwt = jwt;
                    _logger.LogInformation($"User {parentCreateDTO.DNI} authenticated");
                    #endregion

                    registeredParents.Add(newParentToReturn);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                    return Results.Problem("Internal error", statusCode: 500);
                }
            }

            return Results.Ok(new
            {
                RegisteredParents = registeredParents
            });
        }

        [HttpGet("parent/{parentId}", Name = "GetParentInfo")]
        public async Task<IResult> GetParent(int parentId)
        {
            //TODO> Poner para que traiga toda la info para consultas> parentid, ubigeo id, etc
            return Results.Ok(new
            {
                Parent = await _parentService.GetParentAsync(parentId)
            });
        }

        [HttpPost("health-personnel", Name = "RegisterHealthPersonnel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "RegisterHealthPersonnel", typeof(int))]
        public async Task<IResult> RegisterHealthPersonnel(HealthPersonnelCreateDTO healthPersonnelCreateDTO)
        {
            try
            {
                var newUser = new User
                {
                    Email = healthPersonnelCreateDTO.Email,
                    FirstName = healthPersonnelCreateDTO.FirstName,
                    LastName = healthPersonnelCreateDTO.LastName,
                    UserName = healthPersonnelCreateDTO.DNI
                };

                await _userManager.CreateAsync(newUser, healthPersonnelCreateDTO.Password);
                await _userManager.AddToRoleAsync(newUser, Roles.HealthPersonnel);
                healthPersonnelCreateDTO.UserId = newUser.Id;

                var newHealthPersonnelId = await _healthPersonnelService.RegisterHealthPersonnelAsync(healthPersonnelCreateDTO);

                if (newHealthPersonnelId < 1)
                {
                    var errorMessage = $"Something was wrong when create the health personnel {healthPersonnelCreateDTO.Email}";
                    _logger.LogError(errorMessage);
                    return Results.BadRequest(errorMessage);
                }

                return Results.Ok(new
                {
                    HealthPersonnelId = newHealthPersonnelId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpPost("register-health-personnels", Name = "RegisterHealthPersonnels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "RegisterHealthPersonnels", typeof(List<RegisteredHealthPersonnelDTO>))]
        public async Task<IResult> RegisterHealthPersonnels(List<HealthPersonnelCreateDTO> healthPersonnelsCreateDTO)
        {
            _logger.LogInformation("Register and authenticate Health Personnel process started");
            List<RegisteredHealthPersonnelDTO> registeredHealthPersonnels = new List<RegisteredHealthPersonnelDTO>();
            foreach (var healthPersonnelCreateDTO in healthPersonnelsCreateDTO)
            {
                try
                {
                    var newHealthPersonnelToReturn = new RegisteredHealthPersonnelDTO();

                    #region Register User
                    var newUser = new User
                    {
                        Email = healthPersonnelCreateDTO.Email,
                        FirstName = healthPersonnelCreateDTO.FirstName,
                        LastName = healthPersonnelCreateDTO.LastName,
                        UserName = healthPersonnelCreateDTO.DNI
                    };

                    await _userManager.CreateAsync(newUser, healthPersonnelCreateDTO.Password);
                    await _userManager.AddToRoleAsync(newUser, Roles.HealthPersonnel);
                    healthPersonnelCreateDTO.UserId = newUser.Id;

                    var newHealthPersonnelId = await _healthPersonnelService.RegisterHealthPersonnelAsync(healthPersonnelCreateDTO);

                    if (newHealthPersonnelId < 1)
                    {
                        var errorMessage = $"Something was wrong when create the health personnel {healthPersonnelCreateDTO.Email}";
                        _logger.LogError(errorMessage);
                        continue;
                    }

                    newHealthPersonnelToReturn.HealthPersonnelId = newHealthPersonnelId;
                    newHealthPersonnelToReturn.UserId = newUser.Id;
                    newHealthPersonnelToReturn.Username = newUser.UserName;
                    #endregion

                    #region Authenticate Users
                    // Verificamos credenciales con Identity
                    var user = await _userManager.FindByNameAsync(healthPersonnelCreateDTO.DNI);

                    if (user is null || !await _userManager.CheckPasswordAsync(user, healthPersonnelCreateDTO.Password))
                    {
                        _logger.LogWarning($"The User {healthPersonnelCreateDTO.DNI} maybe does not exist or their password is incorrect (Error 403 - Wrong credentials)");
                    }

                    var roles = await _userManager.GetRolesAsync(user);

                    // Generamos un token según los claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                    };
                    bool entityInfoObtained = false;
                    string roleToReturn = "";
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        if (!entityInfoObtained)
                        {
                            int entityId = 0;
                            string entityDNI = "";
                            if (role == Roles.Parent)
                            {
                                var result = await _userService.GetParentByUserIdAsync(user.Id);
                                entityId = result.ParentId;
                                entityDNI = result.DNI;
                                roleToReturn = Roles.Parent;
                            }
                            else if (role == Roles.HealthPersonnel)
                            {
                                var result = await _userService.GetHealthPersonnelByUserIdAsync(user.Id);
                                entityId = result.HealthPersonnelId;
                                entityDNI = result.DNI;
                                roleToReturn = Roles.HealthPersonnel;
                            }
                            else
                            {
                                return Results.Problem();
                            }
                            claims.Add(new Claim(CustomClaimTypes.EntityId, entityId.ToString()));
                            claims.Add(new Claim(CustomClaimTypes.DNI, entityDNI));
                            entityInfoObtained = true;
                        }
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
                    newHealthPersonnelToReturn.Jwt = jwt;
                    _logger.LogInformation($"User {healthPersonnelCreateDTO.DNI} authenticated");
                    #endregion

                    registeredHealthPersonnels.Add(newHealthPersonnelToReturn);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                    return Results.Problem("Internal error", statusCode: 500);
                }
            }

            return Results.Ok(new
            {
                RegisteredHealthPersonnels = registeredHealthPersonnels
            });
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