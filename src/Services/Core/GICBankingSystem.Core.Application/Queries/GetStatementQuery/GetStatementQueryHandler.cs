using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Queries.GetStatementQuery;

public class GetStatementQueryHandler : IQueryHandler<GetStatementQuery, GetStatementResult>
{
    private readonly ITransactionService _transactionService;

    public GetStatementQueryHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<GetStatementResult> Handle(GetStatementQuery query, CancellationToken cancellationToken)
    {
        var statement = await _transactionService.GetMonthlyStatement(query.AccountNo, query.StartDate);
        return new GetStatementResult(statement);
    }
}