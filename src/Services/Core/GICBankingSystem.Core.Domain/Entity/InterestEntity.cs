using GICBankingSystem.Core.Domain.Exceptions;

namespace GICBankingSystem.Core.Domain.Models;

public class InterestEntity
{
    public string RuleId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal Rate { get; set; }

}
