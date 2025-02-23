namespace GICBankingSystem.Console.Models
{
    public class InterestRule
    {
        public string EffectiveDate { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
    }

    public class InterestRulesResponse
    {
        public List<InterestRule> Response { get; set; }
    }
}
