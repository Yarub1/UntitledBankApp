using System;
using System.Collections.Generic;
using System.Text;
using UntitledBankApp.Models;
using UntitledBankApp.Views.Utilities;

/// <summary>
/// This class represents the view for a client in the banking application.
/// </summary>
public class ClientView : View
{
    private Client _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientView"/> class.
    /// </summary>
    /// <param name="client">The client object.</param>
    public ClientView(Client client)
    {
        _client = client;
    }

    /// <summary>
    /// Displays the header for the client view.
    /// </summary>
    protected override void DisplayHeader()
    {
        Console.WriteLine($"{ConsoleColors.Cyan}LOGGED IN AS CLIENT {_client.Username}{ConsoleColors.Reset}");
    }

    /// <summary>
    /// Displays the client's accounts.
    /// </summary>
    /// <param name="accounts">The list of client accounts.</param>
    public void DisplayClientAccounts(List<Account> accounts)
    {
        // Check if accounts exist
        if (accounts != null && accounts.Count > 0)
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine($"{ConsoleColors.White}Client: {_client.FullName}{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}┌─────────────────────────────────────────────────────────────────────────┐{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}│ Account Type  Account Number  Balance              Interest Rate        │{ConsoleColors.Reset}");
            tableBuilder.AppendLine($"{ConsoleColors.Blue}├─────────────────────────────────────────────────────────────────────────┤{ConsoleColors.Reset}");

            // Iterate through each account
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
    }

    /// <summary>
    /// Displays the client's accounts and additional information.
    /// </summary>
    /// <param name="accounts">The list of client accounts.</param>
    public void DisplayClientAccountsWithUpdate(List<Account> accounts)
    {
        // Display client accounts
        DisplayClientAccounts(accounts);

        // Display client information
        ClientInformation(accounts);
    }

    /// <summary>
    /// Represents the transfer options for the client.
    /// </summary>
    public enum TransferOption
    {
        TransferBetweenYourAccounts = 1,
        ViewTransferHistory = 2,
        Invalid = 3,
    }

    /// <summary>
    /// Displays the client's information.
    /// </summary>
    /// <param name="accounts">The list of client accounts.</param>
    public void ClientInformation(List<Account> accounts)
    {
        Console.WriteLine($"{ConsoleColors.Magenta}[Client Information:]{ConsoleColors.Reset}\n.........................");
        Console.WriteLine($"{ConsoleColors.White}Full Name: {_client.FullName}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Client ID: {accounts.Count}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Email: {_client.Email}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Address: {_client.Address}{ConsoleColors.Reset}");
        Console.WriteLine($"{ConsoleColors.White}Telephone Number: {_client.TelephoneNumber}{ConsoleColors.Reset}");
    }
}
