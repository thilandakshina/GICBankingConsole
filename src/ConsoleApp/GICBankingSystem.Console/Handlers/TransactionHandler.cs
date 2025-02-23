using System.Globalization;
using GICBankingSystem.Console.Constants;
using GICBankingSystem.Console.Models;
using GICBankingSystem.Console.Services;

namespace GICBankingSystem.Console.Handlers;

public class TransactionHandler
{
    private readonly IBankingService _bankingService;

    public TransactionHandler(IBankingService bankingService)
    {
        _bankingService = bankingService;
    }

    public async Task HandleTransactions()
    {
        while (true)
        {
            System.Console.WriteLine(Messages.Prompts.TransactionDetails);
            System.Console.WriteLine(Messages.Prompts.BackToMainMenu);
            System.Console.Write(">");

            var input = System.Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                break;

            if (!TryParseTransactionInput(input, out var request))
                continue;

            try
            {
                var response = await _bankingService.ProcessTransactionAsync(request);
                DisplayTransactionResponse(response);
                System.Console.WriteLine(Messages.AnythingElse);
                break;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{ex.Message}");
            }
        }
    }

    private bool TryParseTransactionInput(string input, out TransactionRequest request)
    {
        request = null;
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 4)
        {
            System.Console.WriteLine(Messages.InvalidFormat);
            return false;
        }

        if (!decimal.TryParse(parts[3], out decimal amount))
        {
            System.Console.WriteLine(Messages.InvalidAmount);
            return false;
        }

        if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime date))
        {
            System.Console.WriteLine(Messages.InvalidDateFormat);
            return false;
        }

        request = new TransactionRequest
        {
            Transaction = new TransactionData
            {
                CreatedDate = date,
                AccountNo = parts[1],
                Type = parts[2].ToUpper(),
                Amount = amount
            }
        };

        return true;
    }

    private void DisplayTransactionResponse(TransactionResponse response)
    {
        if (response?.Response == null) return;

        System.Console.WriteLine($"\nAccount: {response.Response.AccountNo}");
        System.Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
        foreach (var tx in response.Response.Transactions)
        {
            System.Console.WriteLine($"| {tx.CreatedDate:yyyyMMdd} | {tx.TransactionId} | {tx.Type}    | {tx.Amount:F2} |");
        }
        System.Console.WriteLine();
    }
}
