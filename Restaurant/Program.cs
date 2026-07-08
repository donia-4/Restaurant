using Restaurant.API;
using Restaurant.Application;
using Restaurant.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddPresentationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultCorsPolicy");

app.UseOutputCache();

app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();