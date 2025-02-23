using GICBankingSystem.Core.Domain.Enums;
using GICBankingSystem.Core.Domain.Exceptions;

namespace GICBankingSystem.Core.Domain.Models;

public class AccountEntity
{
    public string AccountNo { get; set; }
    public DateTime CreatedDate { get; set; }

    public void Add(string accountNo, DateTime date)
    {
        AccountNo = accountNo;
        CreatedDate = date;
    }
}