using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Commands.ProcessTransaction;

public class ProcessTransactionCommandHandler : ICommandHandler<ProcessTransactionCommand, ProcessTransactionResult>
{
    private readonly ITransactionService _transactionService;

    public ProcessTransactionCommandHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<ProcessTransactionResult> Handle(ProcessTransactionCommand command, CancellationToken cancellationToken)
    {
        var statement = await _transactionService.ProcessTransaction(command.Transaction, cancellationToken);
        return new ProcessTransactionResult(statement);
    }
}