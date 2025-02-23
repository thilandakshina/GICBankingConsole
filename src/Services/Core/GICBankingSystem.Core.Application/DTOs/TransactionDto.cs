namespace GICBankingSystem.Core.Application.DTOs;

[ExcludeFromCodeCoverage]
public record TransactionDto
{
    public DateTime CreatedDate { get; set; }
    public string AccountNo { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
}