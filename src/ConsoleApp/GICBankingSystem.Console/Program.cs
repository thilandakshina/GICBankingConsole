using GICBankingSystem.Console.Constants;
using GICBankingSystem.Console.Handlers;
using GICBankingSystem.Console.Services;

namespace GICBankingSystem.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var bankingService = new BankingService();
            var menuHandler = new MenuHandler(bankingService);

            System.Console.WriteLine(Messages.Welcome);

            while (true)
            {
                try
                {
                    menuHandler.DisplayMainMenu();
                    var choice = System.Console.ReadLine();
                    await menuHandler.HandleMenuChoice(choice);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}