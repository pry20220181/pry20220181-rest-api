using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_data_layer;
using pry20220181_data_layer.Utils;
using pry20220181_rest_api.Controllers;
using pry20220181_rest_api.Security.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database Configuration
#region Configure In Memory DB
builder.Services.AddDbContext<PRY20220181DbContext>(options =>
{
    options.UseInMemoryDatabase("pry20220181db");
});
#endregion

#region Configure MySQL DB
////TODO: Get from AppSettings.json
//var connectionString = "server=localhost;user=root;password=servidor;database=pry20220181";

//// Replace 'YourDbContext' with the name of your own DbContext derived class.
//builder.Services.AddDbContext<PRY20220181DbContext>(dbContextOptions => dbContextOptions
//        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
//// The following three options help with debugging, but should
//// be changed or removed for production.
////.LogTo(Console.WriteLine, LogLevel.Information)
////.EnableSensitiveDataLogging()
////.EnableDetailedErrors()
//);
#endregion
#endregion

#region Configure Authentication
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PRY20220181DbContext>();

builder.Services.Configure<IdentityOptions>(options =>
	{
		// Password settings
		options.Password.RequireDigit = false;
		options.Password.RequiredLength = 4;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	});

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
// set ClockSkew is zero to anule the default value: 5 min
            ClockSkew = TimeSpan.Zero
};
    });
#endregion

#region Configure App Settings Objects
builder.Services.Configure<JwtSection>(configuration.GetSection("Jwt"));
builder.Services.AddScoped(sp => sp.GetService<IOptionsSnapshot<JwtSection>>().Value);

builder.Services.Configure<BlockchainClientConfiguration>(configuration.GetSection("BlockchainService"));
builder.Services.AddScoped(sp => sp.GetService<IOptionsSnapshot<BlockchainClientConfiguration>>().Value);
#endregion

#region Configure my custom classes
builder.Services.AddPRY20220181Repositories();
builder.Services.AddPRY20220181Services();
#endregion

builder.Services.AddCors( options => {
                options.AddPolicy("AllowMyApp", policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors("AllowMyApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
