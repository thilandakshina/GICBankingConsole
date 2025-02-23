using GICBankingSystem.Core.Application.Interfaces.Repository;
using GICBankingSystem.Core.Domain.Models;
//using GICBankingSystem.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GICBankingSystem.Core.Infrastructure.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccountEntity> GetByAccountNoAsync(string accountNo)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNo == accountNo);
    }

    public async Task<int> GetTransactionCountAsync(DateTime date , string accountNo)
    {
        var transactionCount = await _context.Transactions
            .Where(t => t.CreatedDate == date && t.AccountNo == accountNo)
            .CountAsync();

        return transactionCount;
    }

    public async Task AddAccountAsync(AccountEntity account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task AddTransactionAsync(TransactionEntity transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<IEnumerable<TransactionEntity>> GetStatementAsync(string accountNo, DateTime startDate, DateTime endDate)
    {
        var transactions = await _context.Transactions
            .Where(t => t.AccountNo == accountNo &&
                t.CreatedDate >= startDate && t.CreatedDate <= endDate
                )
            .ToListAsync();

        return transactions;

    }
}
