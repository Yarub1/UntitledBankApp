using System;
using UntitledBankApp.Models;
using UntitledBankApp.Views;
using UntitledBankApp.Views.Utilities;
namespace UntitledBankApp.Services;

public class ClientService
{
    private PseudoDb _pseudoDb;
    private List<Account> accounts;
    private List<Account> clientAccounts;
    private List<Client> clients = new List<Client>();

    // Constructor that takes PseudoDb as a parameter
    public ClientService(PseudoDb pseudoDb)
    {
        _pseudoDb = pseudoDb;
    }

    public bool OpenAccount(Client client, CurrencyCode code, string accountType, decimal initialDeposit = 0)
    {
        const int minAccountValue = 1000000000;
        const int maxAccountValue = 2147483646;
        const int diffAccountValue = maxAccountValue - minAccountValue;

        if (MaxAccountsReached(diffAccountValue))
        {
            Console.WriteLine("Cannot open a new account. Maximum account limit reached.");
            return false;
        }

        // Generate a new account number
        var number = GenerateAccountNumber(minAccountValue, maxAccountValue);

        // Get the currency based on the provided currency code
        var currency = _pseudoDb.Currencies[code];

        // Ensure that client and client.Accounts are not null before accessing
        if (client == null || client.Accounts == null)
        {
            Console.WriteLine("Invalid client. Cannot open an account for a null client.");
            return false;
        }

        // Create a new account
        var account = new Account(accountType, number, client, new Balance(0, currency), 1.01m);

        // Add the account to the client's list of accounts
        client.Accounts.Add(account);

        // If it's a savings account, calculate interest and record the transfer
        if (accountType.Equals(Account.Savings, StringComparison.OrdinalIgnoreCase))
        {
            decimal interestRate = 0.02m; // 2%
            var interestAmount = initialDeposit * interestRate;
            account.Balance.Amount += initialDeposit + interestAmount;
            Console.WriteLine($"Congratulations! You earned {interestAmount:C} in interest on your savings.");

            // Record the deposit and interest in the transfer history
            RecordTransfer(account.Number, account.Number, initialDeposit, true);
            RecordTransfer(account.Number, account.Number, interestAmount, true);
        }

        // Save the user and account in the pseudo database
        if (!_pseudoDb.Users.Contains(client))
        {
            _pseudoDb.Users.Add(client);
        }

        if (!_pseudoDb.Accounts.Contains(account))
        {
            _pseudoDb.Accounts.Add(account);
        }

        return true;
    }
    private bool MaxAccountsReached(int diffAccountValue)
    {
        var currentAccountCount = _pseudoDb.Users.OfType<Client>().SelectMany(client => client.Accounts).Count();

        return currentAccountCount >= diffAccountValue;
    }

    private int GenerateAccountNumber(int minAccountValue, int maxAccountValue)
    {
        int number;
        
        do
        {
            var random = new Random();
            number = random.Next(minAccountValue, maxAccountValue + 1); // +1 because maxValue is exclusive
        } while (CheckIfNumberIsUnique(number));

        return number;
    }

    private bool CheckIfNumberIsUnique(int number)
    {
        return _pseudoDb.Users.OfType<Client>().Any(client => client.Accounts.Any(account => account.Number == number));
    }

    //............................................................................
    public bool RequestLoan(Account account, decimal amount)
    {
        if (account == null || account.Owner == null)
        {
            Console.WriteLine("Invalid account or account owner. Unable to process the loan request.");
            return false;
        }

        decimal totalBalance = account.Owner.Accounts.Sum(a => a.Balance.Amount);

        if (account.Owner.loanCap == 0 && totalBalance != 0)
        {
            account.Owner.loanCap = totalBalance * 6;
        }

        if ((amount + account.Owner.debt) <= account.Owner.loanCap)
        {
            // Internal transfer logic
            account.Balance.Amount += amount;
            account.Owner.debt += amount;

            RecordTransfer(account.Number, account.Number, amount, true);

            Console.WriteLine($"Loan approved for Account ID: {account.Number}");

            // Print the balance after loan approval
            Console.WriteLine($"Balance after loan: {account.Balance.Amount}");

            // Update the balance in the pseudo database
            var dbAccount = _pseudoDb.Users
                .OfType<Client>()
                .SelectMany(client => client.Accounts)
                .FirstOrDefault(a => a.Number == account.Number);

            if (dbAccount != null)
            {
                dbAccount.Balance.Amount = account.Balance.Amount;
            }


            return true;
        }
        else
        {
            Console.WriteLine($"Unable to process the loan request. Insufficient loan capacity for Account ID: {account.Number}");
            return false;
        }
    }




    public List<Account> GetClientAccounts(Client client)
    {
        return client?.Accounts ?? new List<Account>();
    }




    public bool Transfer(int sourceAccountNumber, decimal amount, int targetAccountNumber, bool isExternalTransfer, out decimal remainingBalance)
    {
        // Find the source account within the bank's clients
        Account sourceAccount = _pseudoDb.Users
            .OfType<Client>()
            .SelectMany(client => client.Accounts)
            .FirstOrDefault(account => account.Number == sourceAccountNumber);
        if (sourceAccount == null)
        {
            throw new ArgumentException("Invalid source account number.");
        }

        if (amount > sourceAccount.Balance.Amount)
        {
            throw new ArgumentException("Insufficient funds in the source account.");
        }

        if (sourceAccount != null && amount <= sourceAccount.Balance.Amount)
        {
            // Find the target account within the bank's clients
            Account targetAccount = _pseudoDb.Users
                .OfType<Client>()
                .SelectMany(client => client.Accounts)
                .FirstOrDefault(account => account.Number == targetAccountNumber);

            if (targetAccount != null)
            {
                // Internal transfer logic
                sourceAccount.Balance.Amount -= amount;
                var convertedAmount = (amount * (1 / sourceAccount.Balance.Currency.Rate)) * targetAccount.Balance.Currency.Rate;
                targetAccount.Balance.Amount += convertedAmount;

                // Set the remaining balance
                remainingBalance = sourceAccount.Balance.Amount;

                // Record the transfer in the transfer history
                RecordTransfer(sourceAccountNumber, targetAccountNumber, amount, true);

                // Return a flag indicating success
                return true;
            }
            else
            {
                // External transfer logic
                decimal externalTransferLimit = 1000; // Example limit, you can set your own
                if (amount > externalTransferLimit || amount > sourceAccount.Balance.Amount)
                {
                    // Set the remaining balance to -1 or any other value indicating failure
                    remainingBalance = -1;

                    // Return a flag indicating failure
                    return false;
                }


                remainingBalance = sourceAccount.Balance.Amount - amount;

                sourceAccount.Balance.Amount -= amount;

                // Record the transfer in the transfer history
                RecordTransfer(sourceAccountNumber, targetAccountNumber, amount, true);

                // Return a flag indicating success
                return true;
            }
        }
        else
        {
            // Set the remaining balance to -1 or any other value indicating failure
            remainingBalance = -1;

            // Return a flag indicating insufficient funds or invalid source account
            return false;
        }
    }
    public List<TransferRecord> GetTransferHistory(int accountNumber)
    {
        // Get the transfer records related to the specified account
        return _pseudoDb.TransferRecords
            .Where(record => record.SourceAccountNumber == accountNumber || record.TargetAccountNumber == accountNumber)
            .ToList();
    }
    public void RecordTransfer(int sourceAccountNumber, int targetAccountNumber, decimal amount, bool transferSuccessful)
    {
        TransferRecord transferRecord = new TransferRecord
        {
            Timestamp = DateTime.Now,
            SourceAccountNumber = sourceAccountNumber,
            TargetAccountNumber = targetAccountNumber,
            Amount = amount,
            TransferSuccessful = transferSuccessful
        };

        _pseudoDb.TransferRecords.Add(transferRecord);
    }


    //................................
    public decimal ConvertAmount(Account sourceAccount, decimal targetAccount, decimal amount)
    {
        decimal convertedAmount = amount * (sourceAccount.Balance.Currency.Rate / targetAccount);

        return convertedAmount;
    }
    public Account AccountByCurrency(int targetAccountNumber, CurrencyCode targetCurrency)
    {
        return _pseudoDb.AllAccounts.FirstOrDefault(account => account.Number == targetAccountNumber && account.Balance.Currency.Code == targetCurrency);
    }


}







