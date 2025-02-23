using System.Reflection;
using FluentValidation;
using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Core.Application.Mappings;
using GICBankingSystem.Core.Application.Services;
using GICBankingSystem.Shared.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GICBankingSystem.Core.Application;

[ExcludeFromCodeCoverage]

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IInterestRuleService, InterestRuleService>();

        services.AddAutoMapper(typeof(InterestMappingProfile));
        services.AddAutoMapper(typeof(TransactionMappingProfile));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
