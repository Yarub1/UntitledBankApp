using System.Text;
using UntitledBankApp;
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
        Console.WriteLine($"LOGGED IN AS CLIENT {_client.Username}");

    }


    public void DisplayClientAccounts(List<Account> accounts)
    {
        if (accounts != null && accounts.Count > 0)
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine($"Client: {_client.FullName}");
            tableBuilder.AppendLine("┌─────────────────────────────────────────────────────────────────────────┐");
            tableBuilder.AppendLine("│ Account Type  Account Number  Balance              Interest Rate        │");
            tableBuilder.AppendLine("├─────────────────────────────────────────────────────────────────────────┤");

            foreach (var account in accounts)
            {
                tableBuilder.AppendLine($"│ {account.AccountType,-13} {account.Number,-15} {account.Balance.Amount,-20:C2} {account.InterestRate,-20:P2} │");
            }

            tableBuilder.AppendLine("└─────────────────────────────────────────────────────────────────────────┘");

            Console.WriteLine(tableBuilder.ToString());
        }
        else
        {
            Console.WriteLine("No accounts found for the client.");
        }
    }


    //........................................................................
  
   
public enum TransferOption
    {
        TransferBetweenYourAccounts =1,
        ViewTransferHistory=2 ,
        Invalid=3,
    }

   
}