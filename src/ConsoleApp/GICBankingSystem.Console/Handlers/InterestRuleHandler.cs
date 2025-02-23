using System.Globalization;
using GICBankingSystem.Console.Constants;
using GICBankingSystem.Console.Models;
using GICBankingSystem.Console.Services;

namespace GICBankingSystem.Console.Handlers;

public class InterestRuleHandler
{
    private readonly IBankingService _bankingService;

    public InterestRuleHandler(IBankingService bankingService)
    {
        _bankingService = bankingService;
    }

    public async Task HandleInterestRules()
    {
        while (true)
        {
            System.Console.WriteLine(Messages.Prompts.InterestRules);
            System.Console.WriteLine(Messages.Prompts.BackToMainMenu);
            System.Console.Write(">");

            var input = System.Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!TryParseInterestRuleInput(input, out var request))
                continue;

            try
            {
                var response = await _bankingService.AddInterestRuleAsync(request);
                DisplayInterestRules(response);
                System.Console.WriteLine(Messages.AnythingElse);
                break;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{ex.Message}");
            }
        }
    }

    private bool TryParseInterestRuleInput(string input, out InterestRuleRequest request)
    {
        request = null;
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            System.Console.WriteLine(Messages.InvalidFormat);
            return false;
        }

        if (!decimal.TryParse(parts[2], out decimal rate))
        {
            System.Console.WriteLine(Messages.InvalidAmount);
            return false;
        }

        if (rate <= 0 || rate >= 100)
        {
            System.Console.WriteLine(Messages.InvalidRate);
            return false;
        }

        if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime effectiveDate))
        {
            System.Console.WriteLine(Messages.InvalidDateFormat);
            return false;
        }

        request = new InterestRuleRequest
        {
            InterestRule = new InterestRuleData
            {
                EffectiveDate = effectiveDate.ToString("yyyy-MM-dd"),
                RuleId = parts[1],
                Rate = rate
            }
        };

        return true;
    }

    private void DisplayInterestRules(List<InterestRule> rules)
    {
        if (rules == null) return;

        System.Console.WriteLine("\nInterest rules:");
        System.Console.WriteLine("| Date     | RuleId | Rate (%) |");
        foreach (var rule in rules.OrderBy(r => r.EffectiveDate))
        {
            System.Console.WriteLine($"| {DateTime.Parse(rule.EffectiveDate):yyyyMMdd} | {rule.RuleId,-6} | {rule.Rate,8:F2} |");
        }
        System.Console.WriteLine();
    }
}
