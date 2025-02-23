using GICBankingSystem.Console.Constants;
using GICBankingSystem.Console.Services;

namespace GICBankingSystem.Console.Handlers
{
    public class MenuHandler
    {
        private readonly TransactionHandler _transactionHandler;
        private readonly InterestRuleHandler _interestRuleHandler;
        private readonly StatementHandler _statementHandler;

        public MenuHandler(IBankingService bankingService)
        {
            _transactionHandler = new TransactionHandler(bankingService);
            _interestRuleHandler = new InterestRuleHandler(bankingService);
            _statementHandler = new StatementHandler(bankingService);
        }

        public void DisplayMainMenu()
        {
            System.Console.WriteLine("[T] Input transactions");
            System.Console.WriteLine("[I] Define interest rules");
            System.Console.WriteLine("[P] Print statement");
            System.Console.WriteLine("[Q] Quit");
            System.Console.Write(">");
        }

        public async Task HandleMenuChoice(string choice)
        {
            switch (choice?.ToUpper().Trim())
            {
                case "T":
                    await _transactionHandler.HandleTransactions();
                    break;
                case "I":
                    await _interestRuleHandler.HandleInterestRules();
                    break;
                case "P":
                    await _statementHandler.HandlePrintStatement();
                    break;
                case "Q":
                    HandleQuit();
                    break;
                default:
                    System.Console.WriteLine(Messages.InvalidOption);
                    break;
            }
        }

        private void HandleQuit()
        {
            System.Console.WriteLine(Messages.Goodbye);
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}
