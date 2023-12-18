using System.Reflection;

namespace UntitledBankApp.Models;

public class Account
{
    public int Number { get; }
    public Client Owner { get; }
    public Balance Balance { get; set; }
    public List<Transfer> Transfers { get; set; }
    public decimal InterestRate { get; set; }
    /// <summary>
    /// Represents the type of the account.
    /// </summary>
    public string AccountType { get; set; }

    /// <summary>
    /// Represents the constant value for savings account type.
    /// </summary>
    public const string Savings = "Savings";

    /// <summary>
    /// Represents the constant value for checking account type.
    /// </summary>
    public const string Checking = "Checking";
    public Account(string accountType, int number, Client client, Balance balance, decimal interestRate)
    {
        AccountType = accountType;
        Number = number;
        Owner = client;
        Balance = balance;
        Transfers = new List<Transfer>();
        InterestRate = interestRate;
    }
}