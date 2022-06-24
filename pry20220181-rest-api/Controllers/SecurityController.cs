using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_rest_api.Security.Models;
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
        private readonly IHealthPersonnelRepository _healthPersonnelRepository;

        public SecurityController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IParentRepository parentRepository, IHealthPersonnelRepository healthPersonnelRepository, IParentService parentService)
        {
            _roleManager = roleManager;

            _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Parent"
            });

            _userManager = userManager;
            _healthPersonnelRepository = healthPersonnelRepository;
            _parentService = parentService;
        }

        [HttpPost("authenticate")]
        public async Task<IResult> Authenticate(AuthenticateRequest request)
        {
            // Verificamos credenciales con Identity
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Forbid();
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

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey")); //TODO: Traer del AppSettings - builder.Configuration["Jwt:Key"]
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: "vnaj.com",//TODO: Traer del AppSettings - builder.Configuration["Jwt:Issuer"],
                audience: "localhost",//TODO: Traer del AppSettings - builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Results.Ok(new
            {
                AccessToken = jwt
            });
        }

        [HttpPost("parent",Name = "RegisterParent")]
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

        //[HttpGet("seeddata")]
        //public async Task<IResult> SeedData()
        //{
        //    var password = "Abc123+";
        //    #region Parent 1
        //    var parent1User = new User
        //    {
        //        Email = "parent1@live.com",
        //        FirstName = "Parent",
        //        LastName = "One",
        //        UserName = "parent1"
        //    };

        //    await _userManager.CreateAsync(parent1User, password);

        //    var parent1 = new Parent
        //    {
        //        DNI = "71222441",
        //        Telephone = "123456789",
        //        UserId = parent1User.Id
        //    };

        //    await _parentRepository.CreateAsync(parent1);
        //    #endregion

        //    #region Parent 2
        //    var parent2User = new User
        //    {
        //        Email = "parent2@live.com",
        //        FirstName = "Parent",
        //        LastName = "Two",
        //        UserName = "parent2"
        //    };

        //    await _userManager.CreateAsync(parent2User, password);

        //    var parent2 = new Parent
        //    {
        //        DNI = "71222442",
        //        Telephone = "123456799",
        //        UserId = parent2User.Id
        //    };

        //    await _parentRepository.CreateAsync(parent2);
        //    #endregion

        //    #region HealthPersonnel 1
        //    var healthPersonnel1User = new User
        //    {
        //        Email = "personnel1@live.com",
        //        FirstName = "Health",
        //        LastName = "Personnel1",
        //        UserName = "personnel1"
        //    };

        //    await _userManager.CreateAsync(healthPersonnel1User, password);

        //    var healthPersonnel1 = new HealthPersonnel
        //    {
        //        UserId = healthPersonnel1User.Id
        //    };

        //    await _healthPersonnelRepository.CreateAsync(healthPersonnel1);
        //    #endregion

        //    #region HealthPersonnel 2
        //    var healthPersonnel2User = new User
        //    {
        //        Email = "personnel2@live.com",
        //        FirstName = "Health",
        //        LastName = "Personnel2",
        //        UserName = "personnel2"
        //    };

        //    await _userManager.CreateAsync(healthPersonnel2User, password);

        //    var healthPersonnel2 = new HealthPersonnel
        //    {
        //        UserId = healthPersonnel2User.Id
        //    };

        //    await _healthPersonnelRepository.CreateAsync(healthPersonnel2);
        //    #endregion

        //    #region Roles
        //    var parentRole = await _roleManager.CreateAsync(new IdentityRole
        //    {
        //        Name = "Parent"
        //    });

        //    var healthPersonnelRole = await _roleManager.CreateAsync(new IdentityRole
        //    {
        //        Name = "HealthPersonnel"
        //    });

        //    await _roleManager.CreateAsync(new IdentityRole
        //    {
        //        Name = "Admin"
        //    });

        //    await _userManager.AddToRoleAsync(parent1User, "Parent");
        //    await _userManager.AddToRoleAsync(parent2User, "Parent");
        //    await _userManager.AddToRoleAsync(healthPersonnel1User, "HealthPersonnel");
        //    await _userManager.AddToRoleAsync(healthPersonnel2User, "HealthPersonnel");
        //    #endregion

        //    return Results.Ok(new
        //    {
        //        Response = "Parents and HealtPersonnel user created"
        //    });
        //}
    }
}
