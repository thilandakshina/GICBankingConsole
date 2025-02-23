namespace GICBankingSystem.Core.Application.DTOs;

[ExcludeFromCodeCoverage]
public record InterestRuleDto
{
    public string RuleId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal Rate { get; set; }

}