using GICBankingSystem.Core.Infrastructure.Data.Repositories;
using GICBankingSystem.Core.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GICBankingSystem.Core.Application.Interfaces.Repository;

namespace GICBankingSystem.Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
