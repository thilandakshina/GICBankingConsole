using AutoMapper;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Core.Domain.Enums;
using GICBankingSystem.Core.Domain.Exceptions;
using GICBankingSystem.Core.Domain.Models;
using GICBankingSystem.Core.Application.Interfaces.Repository;

namespace GICBankingSystem.Core.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StatementDto> ProcessTransaction(TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var account = await GetOrCreateAccount(transactionDto);
            var transactionCount = await _unitOfWork.AccountRepository.GetTransactionCountAsync(transactionDto.CreatedDate, transactionDto.AccountNo);

            var transactionId = $"{transactionDto.CreatedDate.ToString("yyyyMMdd")}-{transactionCount + 1}";
            var transaction = CreateTransactionEntity(transactionDto, transactionId);

            await _unitOfWork.AccountRepository.AddTransactionAsync(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            //validate balance

            return await GetStatement(
                transactionDto.AccountNo,
                account.CreatedDate,
                DateTime.Now
            );
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<StatementDto> GetStatement(string accountNo, DateTime fromDate, DateTime toDate)
    {
        var transactions = await _unitOfWork.AccountRepository.GetStatementAsync(accountNo, fromDate, toDate);

        decimal runningBalance = 0;
        var statementLines = transactions
            .OrderBy(t => t.CreatedDate)
            .Select(txn =>
            {
                var amount = txn.Type == TransactionType.Deposit ? txn.Amount : -txn.Amount;
                runningBalance += amount;

                var statementLine = _mapper.Map<StatementLineDto>(txn);
                statementLine.Balance = runningBalance;
                return statementLine;
            })
            .ToList();

        return new StatementDto
        {
            AccountNo = accountNo,
            StartingBalance = 0,
            Transactions = statementLines,
            FinalBalance = runningBalance
        };
    }

    public async Task<AccountEntity> GetOrCreateAccount(TransactionDto transaction)
    {
        var account = await _unitOfWork.AccountRepository.GetByAccountNoAsync(transaction.AccountNo);
        if (account == null)
        {
            if (transaction.Type.ToUpper() == "W")
                throw new DomainException("First transaction cannot be a withdrawal");

            account = new AccountEntity();
            account.Add(transaction.AccountNo, transaction.CreatedDate);
            await _unitOfWork.AccountRepository.AddAccountAsync(account);
        }
        return account;
    }

    private TransactionEntity CreateTransactionEntity(TransactionDto dto, string transactionId)
    {
        var transaction = _mapper.Map<TransactionEntity>(dto);
        transaction.TransactionId = transactionId;
        return transaction;
    }

    public async Task<StatementDto> GetMonthlyStatement(string accountNo, DateTime startDate)
    {
        var account = await _unitOfWork.AccountRepository.GetByAccountNoAsync(accountNo)
            ?? throw new DomainException($"Account {accountNo} not found");

        var endDate = startDate.AddMonths(1).AddDays(-1);

        var allTransactions = await _unitOfWork.AccountRepository.GetStatementAsync(
            accountNo,
            account.CreatedDate,
            endDate);

        var startingBalance = CalculateStartingBalance(allTransactions, startDate);
        var monthlyTransactions = GetMonthlyTransactions(allTransactions, startDate, startingBalance);

        var interest = await CalculateMonthlyInterest(accountNo, startDate, endDate, startingBalance);
        var finalBalance = AddInterestTransaction(monthlyTransactions, interest, endDate);

        return new StatementDto
        {
            AccountNo = accountNo,
            StartingBalance = startingBalance,
            Transactions = monthlyTransactions,
            FinalBalance = finalBalance
        };
    }

    private decimal CalculateStartingBalance(IEnumerable<TransactionEntity> transactions, DateTime startDate)
    {
        return transactions
            .Where(t => t.CreatedDate < startDate)
            .Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount);
    }

    private List<StatementLineDto> GetMonthlyTransactions(
        IEnumerable<TransactionEntity> allTransactions,
        DateTime startDate,
        decimal startingBalance)
    {
        var statementLines = new List<StatementLineDto>();
        decimal runningBalance = startingBalance;

        var monthTransactions = allTransactions
            .Where(t => t.CreatedDate.Year == startDate.Year && t.CreatedDate.Month == startDate.Month)
            .OrderBy(t => t.CreatedDate);

        foreach (var txn in monthTransactions)
        {
            runningBalance += txn.Type == TransactionType.Deposit ? txn.Amount : -txn.Amount;
            statementLines.Add(_mapper.Map<StatementLineDto>(txn) with { Balance = runningBalance });
        }

        return statementLines;
    }

    public async Task<decimal> CalculateMonthlyInterest(
        string accountNo,
        DateTime startDate,
        DateTime endDate,
        decimal startingBalance)
    {
        var interestRules = await _unitOfWork.InterestRepository.GetByDateRangeAsync(startDate.AddMonths(-5), endDate);
        var orderedRules = interestRules.OrderBy(r => r.EffectiveDate).ToList();

        decimal totalInterest = 0;
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var applicableRule = orderedRules
                .Where(r => r.EffectiveDate <= currentDate)
                .LastOrDefault();

            if (applicableRule != null)
            {
                var transactions = await _unitOfWork.AccountRepository.GetStatementAsync(
                    accountNo,
                    startDate,
                    currentDate);

                decimal endOfDayBalance = startingBalance + transactions
                    .Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount);

                if (endOfDayBalance > 0)
                {
                    decimal dailyRate = applicableRule.Rate / 100 / 365;
                    totalInterest += endOfDayBalance * dailyRate;
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        return Math.Round(totalInterest, 2);
    }

    private decimal AddInterestTransaction(List<StatementLineDto> transactions, decimal interest, DateTime endDate)
    {
        var finalBalance = transactions.LastOrDefault()?.Balance ?? 0;
        finalBalance += interest;

        if (interest != 0)
        {
            transactions.Add(new StatementLineDto
            {
                CreatedDate = endDate,
                TransactionId = string.Empty,
                Type = "I",
                Amount = interest,
                Balance = finalBalance
            });
        }

        return finalBalance;
    }
}
