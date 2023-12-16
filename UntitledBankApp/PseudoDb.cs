using UntitledBankApp.Models;

namespace UntitledBankApp;

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
        { CurrencyCode.EUR, new Currency(CurrencyCode.EUR, 0.9m) },
        { CurrencyCode.SEK, new Currency(CurrencyCode.SEK, 10.41m) },
        { CurrencyCode.USD, new Currency(CurrencyCode.USD, 1) }
    };

    public List<User> Users = new()
    {
        new Admin("AdminFullName", "1", "1", "1", "Email@Email.com", "Email@Email.com", "ABCD", "123456"),
        new Client("ClientFullName", "2", "2", "2", "Email@Email.com", "Email@Email.com", "ABCD", "123456")
        {
            Address = "ClientAddress",
            TelephoneNumber = "123456",// 
             Accounts = new List<Account>
        {
            new Account(Account.Savings, 1, null, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m),
            new Account(Account.Checking, 2, null, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m),


            // Add more accounts as needed
        }
        }

    };
    //public List<User> Users { get; } = new List<User>
    //{
    //    new Admin("AdminFullName", "1", "1", "1", "Email@Email.com", "Email@Email.com", "ABCD", "123456"),

    //};
    public PseudoDb()
    {
        client = new Client("ClientFullName", "2", "2", "2", "Email@Email.com", "Email@Email.com", "ABCD", "123456")
        {
            Address = "ClientAddress",
            TelephoneNumber = "123456",
            Accounts = new List<Account>
        {
            new Account(Account.Savings, 1, client, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m),
            new Account(Account.Checking, 2, client, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m),
            // Add more accounts as needed
        }
        };

        accountSavings = new Account(Account.Savings, 1, client, new Balance(1000.0m, new Currency(CurrencyCode.USD, 1.0m)), 1.2m);
        accountChecking = new Account(Account.Checking, 2, client, new Balance(500.0m, new Currency(CurrencyCode.SEK, 1.0m)), 2.0m);

        Users.Add(client);
    }


    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public List<Account> AllAccounts
    {
        get
        {
            // Combine all accounts from all clients into a single list
            return Users.OfType<Client>().SelectMany(client => client.Accounts).ToList();

        }
    }

    public void AddAccount(Account account)
    {
        Accounts.Add(account);
    }

    public List<Account> GetAccountsForUser(User user)
    {
        if (user is Client client)
        {
            return client.Accounts;
        }

        return new List<Account>();
    }
}