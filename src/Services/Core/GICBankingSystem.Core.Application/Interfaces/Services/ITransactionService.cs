using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<StatementDto> ProcessTransaction(TransactionDto transaction, CancellationToken cancellationToken);
    Task<StatementDto> GetStatement(string accountNo, DateTime fromDate, DateTime toDate);
    Task<AccountEntity> GetOrCreateAccount(TransactionDto transaction);
    Task<StatementDto> GetMonthlyStatement(string accountNo, DateTime startDate);
    Task<decimal> CalculateMonthlyInterest(string accountNo, DateTime startDate, DateTime endDate, decimal startingBalance);
}
