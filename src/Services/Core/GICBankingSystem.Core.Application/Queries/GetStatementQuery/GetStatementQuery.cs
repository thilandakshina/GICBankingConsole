using FluentValidation;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Queries.GetStatementQuery;

public record GetStatementQuery(string AccountNo, DateTime StartDate)
: IQuery<GetStatementResult>;

public record GetStatementResult(StatementDto Statement);

public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
{
    public GetStatementQueryValidator()
    {
        RuleFor(x => x.AccountNo)
            .NotEmpty().WithMessage("Account number is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Date is required")
            .WithMessage("Date must be in YYYYMMdd format");
    }
}