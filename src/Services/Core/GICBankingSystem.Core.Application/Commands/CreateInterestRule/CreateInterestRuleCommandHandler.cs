using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Commands.CreateInterestRule;

public class CreateInterestRuleCommandHandler : ICommandHandler<CreateInterestRuleCommand, CreateInterestRuleResult>
{
    private readonly IInterestRuleService _interestService;

    public CreateInterestRuleCommandHandler(IInterestRuleService interestService)
    {
        _interestService = interestService;
    }

    public async Task<CreateInterestRuleResult> Handle(CreateInterestRuleCommand command, CancellationToken cancellationToken)
    {
        var rates = await _interestService.CreateInterestRule(command.InterestRule, cancellationToken);
        return new CreateInterestRuleResult(rates);
    }
}