namespace GICBankingSystem.Console.Models
{
    public class InterestRuleRequest
    {
        public InterestRuleData InterestRule { get; set; }
    }

    public class InterestRuleData
    {
        public string EffectiveDate { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
    }
}
