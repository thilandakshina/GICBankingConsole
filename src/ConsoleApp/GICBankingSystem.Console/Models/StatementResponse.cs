namespace GICBankingSystem.Console.Models;

public class StatementResponse
{
    public string AccountNo { get; set; }
    public List<StatementTransaction> Transactions { get; set; }
    public decimal StartingBalance { get; set; }
    public decimal FinalBalance { get; set; }
}

public class StatementTransaction
{
    public DateTime CreatedDate { get; set; }
    public string TransactionId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
}
