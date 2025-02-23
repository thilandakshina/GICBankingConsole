using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Interfaces.Repository;

public interface IInterestRepository
{
    Task<IEnumerable<InterestEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<InterestEntity>> GetAllAsync();
    Task AddTransactionAsync(InterestEntity interest);
    Task<InterestEntity> GetByRuleIdAsync(string ruleId);

}
