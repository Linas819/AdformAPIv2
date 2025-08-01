using AdformAPI.AdformDB;
using AdformAPI.Repositories;
using AdformAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AdForm SQL API",
        Description = "An ASP.NET Core Web API for AdForm SQL exercise"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/AdFormLog-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var connectionString = builder.Configuration.GetConnectionString("PostgreContainer") ??
        throw new InvalidOperationException("Connection string 'PostgreContainer' not found");
builder.Services.AddDbContext<AdformDatabaseContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<OrderService, OrderService>();
builder.Services.AddScoped<OrderRepository, OrderRepository>();
builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<ProductRepository, ProductRepository>();

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
