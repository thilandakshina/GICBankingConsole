namespace GICBankingSystem.Core.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TransactionController : Controller
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> ProcessTransaction([FromBody] ProcessTransactionCommand command)
    {
        var result = await _mediator.Send(command);
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
        return Ok(new { response });
    }
}
