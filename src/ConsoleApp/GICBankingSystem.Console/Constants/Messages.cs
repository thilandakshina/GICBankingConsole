namespace GICBankingSystem.Console.Constants;

public static class Messages
{
    public const string Welcome = "\nWelcome to AwesomeGIC Bank! What would you like to do?";
    public const string Goodbye = "\nThank you for banking with AwesomeGIC Bank.\nHave a nice day!";
    public const string InvalidOption = "Invalid option. Please try again.";
    public const string InvalidFormat = "Invalid format. Please try again.";
    public const string InvalidAmount = "Invalid amount format. Please enter a valid number.";
    public const string InvalidRate = "Interest rate should be greater than 0 and less than 100.";
    public const string InvalidDateFormat = "Invalid date format. Please use YYYYMMdd format.";
    public const string AnythingElse = "\nIs there anything else you'd like to do?";

    public static class Prompts
    {
        public const string TransactionDetails = "\nPlease enter transaction details in <Date> <Account> <Type> <Amount> format";
        public const string InterestRules = "\nPlease enter interest rules details in <Date> <RuleId> <Rate in %> format";
        public const string PrintStatement = "\nPlease enter account and month to generate the statement <Account> <Year><Month>";
        public const string BackToMainMenu = "(or enter blank to go back to main menu):";
    }
}
