using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Shared.CQRS;

namespace GICBankingSystem.Core.Application.Queries.GetStatementQuery;

public class GetInterestRuleByDateRangeHandler
    : IQueryHandler<GetInterestRuleByDateRangeQuery, GetInterestRuleByDateRangeResult>
{
    private readonly IInterestRuleService _interestService;

    public GetInterestRuleByDateRangeHandler(IInterestRuleService interestService)
    {
        _interestService = interestService;
    }

    public async Task<GetInterestRuleByDateRangeResult> Handle(
        GetInterestRuleByDateRangeQuery query,
        CancellationToken cancellationToken)
    {
        var rules = await _interestService.GetInterestRulesByDateRange(
            query.FromDate,
            query.ToDate
        );

        return new GetInterestRuleByDateRangeResult(rules);
    }
}
