using System.Text.RegularExpressions;
using GICBankingSystem.Console.Constants;
using GICBankingSystem.Console.Models;
using GICBankingSystem.Console.Services;

namespace GICBankingSystem.Console.Handlers;

public class StatementHandler
{
    private readonly IBankingService _bankingService;
    private readonly Regex _dateFormatRegex = new(@"^\d{6}$");

    public StatementHandler(IBankingService bankingService)
    {
        _bankingService = bankingService;
    }

    public async Task HandlePrintStatement()
    {
        while (true)
        {
            System.Console.WriteLine(Messages.Prompts.PrintStatement);
            System.Console.WriteLine(Messages.Prompts.BackToMainMenu);
            System.Console.Write(">");

            var input = System.Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!TryParseStatementInput(input, out var accountNo, out var period))
                continue;

            try
            {
                var response = await _bankingService.GetStatementAsync(accountNo, period);
                DisplayStatement(response);
                System.Console.WriteLine(Messages.AnythingElse);
                break;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{ex.Message}");
            }
        }
    }

    private bool TryParseStatementInput(string input, out string accountNo, out string period)
    {
        accountNo = null;
        period = null;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            System.Console.WriteLine(Messages.InvalidFormat);
            return false;
        }

        if (!_dateFormatRegex.IsMatch(parts[1]))
        {
            System.Console.WriteLine("Invalid date format. Please use YYYYMM format.");
            return false;
        }

        accountNo = parts[0];
        var year = parts[1].Substring(0, 4);
        var month = parts[1].Substring(4, 2);
        period = $"{year}-{month}-01";

        return true;
    }

    private void DisplayStatement(StatementResponse statement)
    {
        if (statement == null) return;

        System.Console.WriteLine($"\nAccount: {statement.AccountNo}");
        System.Console.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");

        foreach (var transaction in statement.Transactions)
        {
            var date = transaction.CreatedDate.ToString("yyyyMMdd");
            var txnId = transaction.TransactionId?.PadRight(10) ?? "".PadRight(10);
            var type = transaction.Type.PadRight(4);
            var amount = transaction.Amount.ToString("F2").PadLeft(7);
            var balance = transaction.Balance.ToString("F2").PadLeft(8);

            System.Console.WriteLine($"| {date} | {txnId} | {type} | {amount} | {balance} |");
        }
        System.Console.WriteLine();
    }
}
