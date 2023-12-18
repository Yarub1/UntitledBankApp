using UntitledBankApp.Models;

namespace UntitledBankApp;

// This class represents a pseudo database that stores various information related to accounts, clients, and currencies.
public class PseudoDb
{
    public List<Account> Accounts { get; set; } = new();  // Collection to store accounts
    public Client client { get; set; }
    public Account accountSavings { get; set; }
    public Account accountChecking { get; set; }
    public List<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();
    public Dictionary<CurrencyCode, Currency> Currencies => _currencies;
    private Dictionary<CurrencyCode, Currency> _currencies = new Dictionary<CurrencyCode, Currency>
    {
        { CurrencyCode.EUR, new Currency(CurrencyCode.EUR, 0.9m) },  // Euro currency with exchange rate 0.9
        { CurrencyCode.SEK, new Currency(CurrencyCode.SEK, 10.41m) },  // Swedish Krona currency with exchange rate 10.41
        { CurrencyCode.USD, new Currency(CurrencyCode.USD, 1) }  // US Dollar currency with exchange rate 1
    };

    public List<User> Users = new()
    {
        new Admin("AdminFullName", "1", "1", "1", "Email@Email.com", "Email@Email.com", "ABCD", "123456"),  // Create an admin user
        new Client("ClientFullName", "2", "2", "2", "Email@Email.com", "Email@Email.com", "ABCD", "123456")  // Create a client user
        {
            Address = "ClientAddress",
            TelephoneNumber = "123456",  // Set the client's address and telephone number
            Accounts = new List<Account>
            {
                new Account(Account.Savings, 1, null, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m),  // Create a savings account with a balance of 1000 USD
                new Account(Account.Checking, 2, null, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m),  // Create a checking account with a balance of 500 SEK
                // Add more accounts as needed
            }
        }
    };

    public PseudoDb()
    {
        client = new Client("ClientFullName", "2", "2", "2", "Email@Email.com", "Email@Email.com", "ABCD", "123456")  // Create a client user
        {
            Address = "ClientAddress",
            TelephoneNumber = "123456",  // Set the client's address and telephone number
            Accounts = new List<Account>
            {
                new Account(Account.Savings, 1, client, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m),  // Create a savings account with a balance of 1000 USD
                new Account(Account.Checking, 2, client, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m),  // Create a checking account with a balance of 500 SEK
                // Add more accounts as needed
            }
        };

        accountSavings = new Account(Account.Savings, 1, client, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m);  // Create a savings account with a balance of 1000 USD
        accountChecking = new Account(Account.Checking, 2, client, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m);  // Create a checking account with a balance of 500 SEK

        Users.Add(client);  // Add the client user to the list of users
    }

    //..........................................................
    // Update the password of a client
    public void UpdateClientPassword(Client client, string newPassword)
    {
        client.Password = newPassword;
    }

    // Update the email of a client
    public void UpdateClientEmail(Client client, string newEmail)
    {
        client.Email = newEmail;
    }
    //...............................................................

    // Add a user to the list of users
    public void AddUser(User user)
    {
        Users.Add(user);
    }

    // Get all accounts from all clients
    public List<Account> AllAccounts
    {
        get
        {
            // Combine all accounts from all clients into a single list
            return Users.OfType<Client>().SelectMany(client => client.Accounts).ToList();
        }
    }

    // Add an account to the list of accounts
    public void AddAccount(Account account)
    {
        Accounts.Add(account);
    }

    // Get the accounts for a specific user
    public List<Account> GetAccountsForUser(User user)
    {
        if (user is Client client)
        {
            return client.Accounts;
        }

        return new List<Account>();
    }
}
