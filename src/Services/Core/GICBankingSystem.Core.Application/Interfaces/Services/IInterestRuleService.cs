using GICBankingSystem.Core.Application.DTOs;

namespace GICBankingSystem.Core.Application.Interfaces.Services
{
    public interface IInterestRuleService
    {
        Task<IEnumerable<InterestRuleDto>> CreateInterestRule(InterestRuleDto interestRule, CancellationToken cancellationToken);
        Task<IEnumerable<InterestRuleDto>> GetAllInterestRules();
        Task<IEnumerable<InterestRuleDto>> GetInterestRulesByDateRange(DateTime fromDate, DateTime toDate);
        Task ValidateInterestRule(string ruleId);
    }
}
