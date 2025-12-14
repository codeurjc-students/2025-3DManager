using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Base configuration and user secrets
var envName = builder.Environment.EnvironmentName;
var isCI = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "CI", StringComparison.OrdinalIgnoreCase);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: builder.Environment.IsDevelopment() || isCI);

// Services
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

builder.Services.AddScoped<IDataSource<MySqlConnection>>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new MySQLDataSource(connectionString, "3DMANAGER");
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IPrinterManager, PrinterManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IGroupManager, GroupManager>();
builder.Services.AddScoped<IPrintManager, PrintManager>();
builder.Services.AddScoped<IFilamentManager, FilamentManager>();
builder.Services.AddScoped<ICatalogManager, CatalogManager>();
builder.Services.AddScoped<IPrinterDbManager, PrinterDbManager>();
builder.Services.AddScoped<IUserDbManager, UserDbManager>();
builder.Services.AddScoped<IGroupDbManager, GroupDbManager>();
builder.Services.AddScoped<IFilamentDbManager, FilamentDbManager>();
builder.Services.AddScoped<IPrintDbManager, PrintDbManager>();
builder.Services.AddScoped<ICatalogDbManager, CatalogDbManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "3DManager API",
        Version = "v1"
    });
});

//JWT configuration

var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "3DManager API v1"));
}

if (!app.Environment.IsEnvironment("CI"))
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapGet("/health", () => Results.Ok("OK"));

app.Run();

public partial class Program { }
