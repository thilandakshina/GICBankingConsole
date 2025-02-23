using FluentValidation;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Commands.CreateInterestRule;

public record CreateInterestRuleCommand(InterestRuleDto InterestRule)
    : ICommand<CreateInterestRuleResult>;

public record CreateInterestRuleResult(IEnumerable<InterestRuleDto> InterestRules);

public class CreateInterestRuleCommandValidator : AbstractValidator<CreateInterestRuleCommand>
{
    public CreateInterestRuleCommandValidator()
    {
        RuleFor(x => x.InterestRule.RuleId)
            .NotEmpty().WithMessage("Account number is required");
       
        RuleFor(x => x.InterestRule.Rate)
            .GreaterThan(0)
            .LessThan(20)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.InterestRule.EffectiveDate)
            .NotEmpty()
            .WithMessage("Date must be in YYYYMMdd format");
    }
}