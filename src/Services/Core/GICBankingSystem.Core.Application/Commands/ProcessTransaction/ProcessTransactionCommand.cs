using FluentValidation;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Commands.ProcessTransaction;

public record ProcessTransactionCommand(TransactionDto Transaction)
    : ICommand<ProcessTransactionResult>;

public record ProcessTransactionResult(StatementDto Statement);

public class ProcessTransactionCommandValidator : AbstractValidator<ProcessTransactionCommand>
{
    public ProcessTransactionCommandValidator()
    {
        RuleFor(x => x.Transaction.AccountNo)
            .NotEmpty().WithMessage("Account number is required");

        RuleFor(x => x.Transaction.Type)
            .NotEmpty()
            .Must(type => type.ToUpper() is "D" or "W")
            .WithMessage("Type must be D for deposit or W for withdrawal");

        RuleFor(x => x.Transaction.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Transaction.CreatedDate)
             .NotEmpty()
             .Must(BeValidDateFormat)
             .WithMessage("Date must be in YYYYMMdd format");
    }

    private bool BeValidDateFormat(DateTime date)
    {
        return date != DateTime.MinValue;
    }
}