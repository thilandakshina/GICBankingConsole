namespace GICBankingSystem.Core.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InterestController : Controller
{
    private readonly IMediator _mediator;

    public InterestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateInterestRule([FromBody] CreateInterestRuleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.InterestRules);
    }


    [HttpGet]
    public async Task<IActionResult> GetInterestRules([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var query = new GetInterestRuleByDateRangeQuery(fromDate, toDate);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
