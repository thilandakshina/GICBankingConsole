namespace GICBankingSystem.Core.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class StatementController : Controller
{
    private readonly IMediator _mediator;

    public StatementController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetStatement([FromQuery] string accountNo, [FromQuery] DateTime period)
    {

        var query = new GetStatementQuery(accountNo, period);
        var result = await _mediator.Send(query);

        var response = new StatementDto
        {
            AccountNo = result.Statement.AccountNo,
            Transactions = result.Statement.Transactions.Select(t => new StatementLineDto
            {
                TransactionId = t.TransactionId,
                Type = t.Type,
                Amount = t.Amount,
                Balance = t.Balance,
                CreatedDate = t.CreatedDate
            }).ToList(),
            FinalBalance = result.Statement.FinalBalance,
            StartingBalance = result.Statement.StartingBalance
        };
        return Ok(response);
    }
}
