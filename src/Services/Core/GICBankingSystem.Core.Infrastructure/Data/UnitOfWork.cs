using GICBankingSystem.Core.Application.Interfaces.Repository;

namespace GICBankingSystem.Core.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly IInterestRepository _interestRepository;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext context, IAccountRepository accountRepository,
        IInterestRepository interestRepository)
    {
        _context = context;
        _accountRepository = accountRepository;
        _interestRepository = interestRepository;
    }

    public IAccountRepository AccountRepository => _accountRepository;
    public IInterestRepository InterestRepository => _interestRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.Database.CommitTransactionAsync();
        }
        catch
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
