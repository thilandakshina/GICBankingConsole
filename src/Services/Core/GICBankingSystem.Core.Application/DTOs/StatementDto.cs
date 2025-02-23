namespace GICBankingSystem.Core.Application.DTOs;

[ExcludeFromCodeCoverage]
public record StatementDto
{
    public string AccountNo { get; set; }
    public List<StatementLineDto> Transactions { get; set; } = new();
    public decimal StartingBalance { get; set; }
    public decimal FinalBalance { get; set; }
}
