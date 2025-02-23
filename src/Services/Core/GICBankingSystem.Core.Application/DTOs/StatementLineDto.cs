namespace GICBankingSystem.Core.Application.DTOs;

[ExcludeFromCodeCoverage]
public record StatementLineDto
{
    public DateTime CreatedDate { get; set; }
    public string TransactionId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }



}
