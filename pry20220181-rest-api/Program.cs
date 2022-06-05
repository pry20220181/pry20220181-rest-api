using Microsoft.EntityFrameworkCore;
using pry20220181_data_layer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure In Memory DB
//builder.Services.AddDbContext<PRY20220181DbContext>(options =>
//{
//    options.UseInMemoryDatabase("pry20220181db");
//});
#endregion

#region Configure MySQL DB
// Replace with your connection string.
var connectionString = "server=localhost;user=root;password=servidor;database=pry20220181";

// Replace with your server version and type.
// Use 'MariaDbServerVersion' for MariaDB.
// Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
// For common usages, see pull request #1233.
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<PRY20220181DbContext>(dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
#endregion
builder.Services.AddPRY20220181Repositories();
builder.Services.AddPRY20220181Services();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
