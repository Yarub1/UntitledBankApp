using System.Net;
using UntitledBankApp;
using UntitledBankApp.Views.Utilities;
using static UntitledBankApp.Presenters.AdminPresenter;

namespace UntitledBankApp.Views;
public class AdminView : View
{
    private Admin _admin;

    public AdminView(Admin admin)
    {
        _admin = admin;

    }

    protected override void DisplayHeader()
    {
        Console.WriteLine($"LOGGED IN AS ADMIN {_admin.Username}");

    }

    public (UserType userType, string fullName, string username, string password, string passwordVerified, string email, string emailVerified, string address, string telephonenumber) CreateUser()
    {
        int userTypeInput = 0; 
        bool isValidInput = false;

        while (!isValidInput)
        {
            string userTypeInputString = InputUtils.GetNonEmptyString("Select user type:\n1. Admin\n2. Client\nEnter the user type (1 or 2): ");

            isValidInput = int.TryParse(userTypeInputString, out userTypeInput);

            if (!isValidInput || (userTypeInput != 1 && userTypeInput != 2))
            {
                Console.WriteLine("Invalid input. Please enter 1 or 2 for the user type.");
                isValidInput = false;
            }
        }

        UserType userType = userTypeInput == 1 ? UserType.Admin : UserType.Client;

        string fullName = InputUtils.GetNonEmptyString("Full Name");
        string username = InputUtils.GetNonEmptyString("Username");
        string password = InputUtils.GetNonEmptyString("Password");
        string passwordVerified = InputUtils.GetNonEmptyString("Verify Password");
        string email = InputUtils.GetNonEmptyString("Email");
        string emailVerified = InputUtils.GetNonEmptyString("Verify Email");
        string address = InputUtils.GetNonEmptyString("Address");
        string telephonenumber = InputUtils.GetNonEmptyString("Telephone Number");

        return (userType, fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber);
    }


    public CurrencyCode GetCurrencyCode()
    {
        return Enum.Parse<CurrencyCode>(InputUtils.GetNonEmptyString
            ("Enter the currency code to update (USD, EUR, SEK)").ToUpper());
    }
    public decimal GetExchangeRate()
    {
        
        return decimal.Parse(InputUtils.GetNonEmptyString("Enter the new exchange rate"));
        
    }

    public decimal GetBorrowingLimit()
    {
       
        return decimal.Parse(InputUtils.GetNonEmptyString("Enter new borrowing limit"));
    }
    public void DisplayCustomerList(IEnumerable<Client> clients)
    {
       Console.WriteLine("Customer List:", ConsoleColor.DarkYellow);
        foreach (var client in clients)
        {
            Console.WriteLine($"Username: {client.Username}, Full Name: {client.FullName}");
        }
    }

    public string ChooseClientFromList(IEnumerable<Client> clients)
    {

       Console.WriteLine("Choose a client from the list:", ConsoleColor.Black);
        DisplayCustomerList(clients);
        return InputUtils.GetNonEmptyString("Enter the username of the client");
    }
}