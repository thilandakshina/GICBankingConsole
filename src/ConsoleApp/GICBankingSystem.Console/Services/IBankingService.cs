using GICBankingSystem.Console.Models;

namespace GICBankingSystem.Console.Services;

public interface IBankingService
{
    Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request);
    Task<List<InterestRule>> AddInterestRuleAsync(InterestRuleRequest request);
    Task<StatementResponse> GetStatementAsync(string accountNo, string period);
}
