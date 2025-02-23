using GICBankingSystem.Core.API;
using GICBankingSystem.Core.Application;
using GICBankingSystem.Core.Infrastructure;
using GICBankingSystem.Shared.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var assembly = typeof(Program).Assembly;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(options => { }); 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
