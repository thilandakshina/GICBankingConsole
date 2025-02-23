using System.Transactions;
using GICBankingSystem.Core.Domain.Enums;
using GICBankingSystem.Core.Domain.Exceptions;

namespace GICBankingSystem.Core.Domain.Models;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public string TransactionId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string AccountNo { get; set; }
    public DateTime CreatedDate { get; set; }
}
