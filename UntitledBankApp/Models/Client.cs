using System.Net;

namespace UntitledBankApp.Models;

public class Client : User
{
    public decimal debt;
    public decimal loanCap;
    public List<Account> Accounts { get; set; }
    public string Address { get; set; }
    public string TelephoneNumber { get; set; }

    public Client(string fullName, string username, string password, string passwordVerified, string email, string emailVerified , string address, string telephonenumber) : base(Role.Client, fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber)
    {
        Accounts = new List<Account>();
        loanCap = 0;
    }
}