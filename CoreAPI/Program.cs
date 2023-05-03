using AppCore;
using AppCore.Contracts.Common;
using CoreAPI.Filters;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Shared.Libraries.Constants;
using Shared.Libraries.Logging;
using Shared.Libraries.Sidecar;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

GlobalConfiguration.Bind(builder.Configuration);


builder.Services.AddControllers(p =>
{
    p.Filters.Add(typeof(ApiExceptionFilterAttribute));
});

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};


// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration[Service.Name],
        Version = "v1"
    });
    c.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
});


builder.Services.AddHttpContextAccessor();

// Application Configuration
builder.Services.AddApplicationServices(builder.Configuration);

// Infrastructure Configuration
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.AddSerilog(builder.Configuration);
builder.Services.AddDapr(builder.Configuration);


builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MigrationDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
