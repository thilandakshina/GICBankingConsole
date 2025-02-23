using FluentValidation;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Queries.GetStatementQuery;

public record GetInterestRuleByDateRangeQuery(DateTime FromDate, DateTime ToDate)
: IQuery<GetInterestRuleByDateRangeResult>;

public record GetInterestRuleByDateRangeResult(IEnumerable<InterestRuleDto> InterestRule);

public class GetInterestRuleByDateRangeValidator : AbstractValidator<GetInterestRuleByDateRangeQuery>
{
    public GetInterestRuleByDateRangeValidator()
    {
       
        RuleFor(x => x.FromDate)
            .NotEmpty().WithMessage("Date is required")
            .WithMessage("Date must be in YYYYMMdd format");

        RuleFor(x => x.ToDate)
           .NotEmpty().WithMessage("Date is required")
           .WithMessage("Date must be in YYYYMMdd format");
    }
}