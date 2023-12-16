using System;
using System.Text;
using UntitledBankApp.Models;
using UntitledBankApp.Views.Utilities;

public class ClientView : View
{
    private Client _client;
    public ClientView(Client client)
    {
        _client = client;
    }

    protected override void DisplayHeader()
    {
        Console.WriteLine($"{ConsoleColors.Cyan}LOGGED IN AS CLIENT {_client.Username}{ConsoleColors.Reset}");
    }

    public void DisplayClientAccounts(List<Account> accounts)
    {
        if (accounts != null && accounts.Count > 0)
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine($"{ConsoleColors.White}Client: {_client.FullName}{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}┌─────────────────────────────────────────────────────────────────────────┐{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}│ Account Type  Account Number  Balance              Interest Rate        │{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}├─────────────────────────────────────────────────────────────────────────┤{ConsoleColors.Reset}");

            foreach (var account in accounts)
            {
                tableBuilder.AppendLine($"{ConsoleColors.Green}│ {account.AccountType,-13} {account.Number,-15} {account.Balance.Amount,-20:C2} {account.InterestRate,-20:P2} │{ConsoleColors.Reset}");
            }

            tableBuilder.AppendLine($"{ConsoleColors.Blue}└─────────────────────────────────────────────────────────────────────────┘{ConsoleColors.Reset}");

            Console.WriteLine(tableBuilder.ToString());
        }
        else
        {
            Console.WriteLine($"{ConsoleColors.Red}No accounts found for the client.{ConsoleColors.Reset}");
        }

        Console.WriteLine($"{ConsoleColors.Magenta}[Client Information:]{ConsoleColors.Reset}\n.........................");
        Console.WriteLine($"{ConsoleColors.White}Full Name: {_client.FullName}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Client ID: {accounts.Count}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Email: {_client.Email}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Address: {_client.Address}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Telephone Number: {_client.TelephoneNumber}{ConsoleColors.Reset}");
    }

    public void DisplayClientAccountsWithUpdate(List<Account> accounts)
    {
        if (accounts != null && accounts.Count > 0)
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine($"{ConsoleColors.White}Client: {_client.FullName}{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}┌─────────────────────────────────────────────────────────────────────────┐{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}│ Account Type  Account Number  Balance              Interest Rate        │{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}├─────────────────────────────────────────────────────────────────────────┤{ConsoleColors.Reset}");

            foreach (var account in accounts)
            {
                tableBuilder.AppendLine($"{ConsoleColors.Green}│ {account.AccountType,-13} {account.Number,-15} {account.Balance.Amount,-20:C2} {account.InterestRate,-20:P2} │{ConsoleColors.Reset}");
            }

            tableBuilder.AppendLine($"{ConsoleColors.Blue}└─────────────────────────────────────────────────────────────────────────┘{ConsoleColors.Reset}");

            Console.WriteLine(tableBuilder.ToString());
        }
        else
        {
            Console.WriteLine($"{ConsoleColors.Red}No accounts found for the client.{ConsoleColors.Reset}");
        }

        Console.WriteLine($"{ConsoleColors.Magenta}[Client Information:]{ConsoleColors.Reset}\n.........................");
        Console.WriteLine($"{ConsoleColors.White}Full Name: {_client.FullName}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Client ID: {accounts.Count}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Email: {_client.Email}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Address: {_client.Address}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Telephone Number: {_client.TelephoneNumber}{ConsoleColors.Reset}");
    }


    public enum TransferOption
    {
        TransferBetweenYourAccounts = 1,
        ViewTransferHistory = 2,
        Invalid = 3,
    }
}
