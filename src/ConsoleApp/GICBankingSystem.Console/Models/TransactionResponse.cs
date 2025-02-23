namespace GICBankingSystem.Console.Models;

public class TransactionResponse
{
    public ResponseData Response { get; set; }
}

public class ResponseData
{
    public string AccountNo { get; set; }
    public List<Transaction> Transactions { get; set; }
    public decimal StartingBalance { get; set; }
    public decimal FinalBalance { get; set; }
}

public class Transaction
{
    public DateTime CreatedDate { get; set; }
    public string TransactionId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
}
