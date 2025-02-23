using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Interfaces.Repository;

public interface IAccountRepository
{
    Task<AccountEntity> GetByAccountNoAsync(string accountNo);
    Task<int> GetTransactionCountAsync(DateTime date, string accountNo);
    Task AddAccountAsync(AccountEntity account);
    Task AddTransactionAsync(TransactionEntity transaction);
    Task<IEnumerable<TransactionEntity>> GetStatementAsync(string accountNo, DateTime startDate, DateTime endDate);

}
