namespace UntitledBankApp.Models;

public class Admin : User
{
    public Admin(string fullName, string username, string password, string passwordVerified , string email , string emailVerified , string address, string telephonenumber ) 
     : base(Role.Admin, fullName, username, password, passwordVerified , email,  emailVerified, address,  telephonenumber)
    {
   
    }
}