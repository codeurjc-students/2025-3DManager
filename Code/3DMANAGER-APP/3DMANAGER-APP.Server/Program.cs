using _3DMANAGER_APP.BLL;
using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Repositories;
using _3DMANAGER_APP.Server.Controllers;
using _3DMANAGER_APP.TestingSupport.Database;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Globalization;
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
    .AddUserSecrets<Program>(optional: true);

// Services
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
    cfg.LicenseKey = builder.Configuration["Automapper:License"];
});
builder.Services.AddScoped<IDataSource<MySqlConnection>>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new MySQLDataSource(connectionString, "3DMANAGER");
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IPrinterService, PrinterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IPrintService, PrintService>();
builder.Services.AddScoped<IFilamentService, FilamentService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPrinterRepository, PrinterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IFilamentRepository, FilamentRepository>();
builder.Services.AddScoped<IPrintRepository, PrintRepository>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "http://localhost:3000", "http://localhost:3001",
            "https://portal-3dmanager-app.agreeablebay-71400cf1.spaincentral.azurecontainerapps.io")
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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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


// Azure Blob Storage
builder.Services.AddSingleton<BlobServiceClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config["AzureStorage:ConnectionString"];
    return new BlobServiceClient(connectionString);
});

builder.Services.AddScoped<IAzureBlobStorageService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var containerName = config["AzureStorage:ContainerName"];
    var blobService = sp.GetRequiredService<BlobServiceClient>();
    var logger = sp.GetRequiredService<ILogger<AzureBlobStorageService>>();

    return new AzureBlobStorageService(blobService, containerName, logger);
});


builder.Services.AddAuthorization();

//SMTP EMAIL SETTINGS
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<IEmailService, EmailService>();

//BackgroundService 
builder.Services.AddHostedService<DailyTaskService>();
builder.Services.AddScoped<IDailyTaskRepository, DailyTaskRepository>();

//Culture
CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
customCulture.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = customCulture;
CultureInfo.DefaultThreadCurrentUICulture = customCulture;


//CI configuration
var app = builder.Build();
if (app.Environment.IsEnvironment("CI"))
{
    using var scope = app.Services.CreateScope();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var cs = config.GetConnectionString("DefaultConnection");

    var seeder = new DatabaseSeeder(cs!);
    await seeder.SeedAsync();
}


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "3DManager API v1"));
}


app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("OK"));

app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"), builder =>
{
    builder.UseRouting();
    builder.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("index.html");
    });
});


app.Run();

public partial class Program { }
