using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using _3DMANAGER.BLL.Managers;
using _3DMANAGER.DAL;
using _3DMANAGER.BLL.Interfaces;
using _3DMANAGER.DAL.Managers;
using _3DMANAGER.DAL.Interfaces;
using AutoMapper;
using _3DMANAGER.BLL.Mapper;
using _3DMANAGER.DAL.Base;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

//Configuration Automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

builder.Services.AddScoped<IDataSource<MySqlConnection>>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new MySQLDataSource(connectionString, "3DMANAGER");
});


//Inyection dependencies
builder.Services.AddScoped<IPrinterManager, PrinterManager>();
builder.Services.AddScoped<IPrinterDbManager, PrinterDbManager>();

//Conection to frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL de tu React en desarrollo
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add logging
builder.Logging.ClearProviders();         
builder.Logging.AddConsole();            
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.MapControllers();


app.Run();


