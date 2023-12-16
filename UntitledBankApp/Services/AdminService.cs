using UntitledBankApp.Factories;
using UntitledBankApp.Views.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UntitledBankApp.Services
{
    public class AdminService
    {
        private PseudoDb _pseudoDb;
        private UserFactory _userFactory;

        public AdminService(PseudoDb pseudoDb, UserFactory userFactory)
        {
            _pseudoDb = pseudoDb;
            _userFactory = userFactory;
        }

        public bool CreateUser(Role role, string fullName, string username, string password, string passwordVerified, string email, string emailVerified, string address, string telephonenumber)
        {
            try
            {
                bool usernameExists = _pseudoDb.Users.Any(u => u.Username == username);

                if (usernameExists)
                {
                    Console.WriteLine($"{ConsoleColors.Red}Username already exists. Please choose a different username.{ConsoleColors.Reset}");
                    return false;
                }

                // Validate email format
                if (!IsValidEmail(email))
                {
                    Console.WriteLine($"{ConsoleColors.Red}Invalid email format. Please enter a valid email address.{ConsoleColors.Reset}");
                    return false;
                }

                // Try to parse phone number
                if (telephonenumber.ToString().Any(char.IsWhiteSpace))
                {
                    Console.WriteLine($"{ConsoleColors.Red}Invalid phone number format. Please enter a valid phone number.{ConsoleColors.Reset}");
                    return false;
                }

                var user = _userFactory.CreateUser(role, fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber);
                _pseudoDb.Users.Add(user);

                // Add the account creation logic here
                var accountNumber = GenerateAccountNumber(); // Implement your logic to generate an account number
                var account = new Account("", accountNumber, user as Client, new Balance(0, _pseudoDb.Currencies[CurrencyCode.USD]), 1.01m);
                _pseudoDb.Accounts.Add(account);

                // Display the account numbers associated with the user
                var userAccounts = _pseudoDb.Accounts.Where(a => a.Owner == user as Client).ToList();
                Console.WriteLine($"{ConsoleColors.Green}User successfully created with the following account numbers:{ConsoleColors.Reset}");
                foreach (var userAccount in userAccounts)
                {
                    Console.WriteLine($"{ConsoleColors.White}Account Number: {userAccount.Number}{ConsoleColors.Reset}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ConsoleColors.Red}An error occurred: {ex.Message}{ConsoleColors.Reset}");
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public int GenerateAccountNumber()
        {
            // Simplified account number generation logic, you may use a more sophisticated algorithm
            return new Random().Next(10000, int.MaxValue);
        }

        public void SetCurrencyRate(CurrencyCode currencyCode, decimal rate)
        {
            if (_pseudoDb.Currencies.ContainsKey(currencyCode))
            {
                _pseudoDb.Currencies[currencyCode].Rate = rate;
            }
            else
            {
                throw new KeyNotFoundException($"{ConsoleColors.Red}Currency '{currencyCode}' not found.{ConsoleColors.Reset}");
            }
        }

        public void SetBorrowingLimit(string username, decimal limit)
        {
            var client = _pseudoDb.Users.OfType<Client>().FirstOrDefault(c => c.Username == username);

            if (client != null)
            {
                client.loanCap = limit;
            }
            else
            {
                throw new KeyNotFoundException($"{ConsoleColors.Red}Client '{username}' not found.{ConsoleColors.Reset}");
            }
        }

        public IEnumerable<Client> GetCustomerList()
        {
            return _pseudoDb.Users.OfType<Client>();
        }
    }
}
