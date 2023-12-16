namespace UntitledBankApp.Models;

public enum Role
{
    Client,
    Admin
}

public abstract class User
{
    public Role Role { get; set; }
    private string _fullName = null!;
    private string _username = null!;
    private string _password = null!;
    private string _email = null!;
    private string _emailVerified = null!;
    private string _passwordVerified = null!;
    private string _address = null!;
    private string _telephonenumber = null;
    public string FullName
    {
        get => _fullName;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to name cannot be null or white space.");
            _fullName = value;
        }
    }
    public string Username
    {
        get => _username;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Username cannot be null or white space.");
            _username = value;
        }
    }
    public string Password
    {
        get => _password;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Password cannot be null or white space.");
            _password = value;
        }
    }


    public string EmailVerified
    {
        get => _emailVerified;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Email Verified cannot be null or white space.");
            _emailVerified = value;
        }
    }

    public string Email
    {
        get => _email;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Email cannot be null or white space.");
            _email = value;
        }
    }
    public string Address
    {
        get => _address;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Address cannot be null or white space.");
            _address = value;
        }
    }

    public string PasswordVerified
    {
        get => _passwordVerified;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("value assigned to Password Verified cannot be null or white space.");
            _passwordVerified = value;
        }
    }
    public string Telephonenumber
    {
        get => _telephonenumber;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("The value assigned to Telephone number cannot contain white spaces.");

            _telephonenumber = value;
        }
    }




    protected User(Role role, string fullName, string username, string password, string passwordVerified, string email, string emailVerified, string address, string telephonenumber)
    {
        Role = role;
        FullName = fullName;
        Username = username;
        Password = password;
        PasswordVerified = passwordVerified;
        Email = email;
        EmailVerified = emailVerified;
        Address = address;

    }
}