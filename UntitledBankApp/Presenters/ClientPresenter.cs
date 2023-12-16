using System;
using System.Security.Principal;
using UntitledBankApp.Models;
using UntitledBankApp.Services;
using UntitledBankApp.Views;
using UntitledBankApp.Views.Utilities;
using static ClientView;

namespace UntitledBankApp.Presenters;

public class ClientPresenter : Presenter
{
    private PseudoDb _pseudoDb;
    private ClientService _clientService;
    private ClientView _clientView;
    private List<TransferRecord> transferRecords = new List<TransferRecord>();
    private Client _client;
    private List<Account> accounts;
    private List<Account> clientAccounts;
    private List<User> Users;
    private InputClient _inputClient { get; set; }
    private Stack<Action> _menuStack;
    private bool _logoutRequested = false;
    public ClientPresenter(PseudoDb pseudoDb, ClientService clientService, ClientView clientView, InputClient inputClient)
    {
        _pseudoDb = pseudoDb;
        _clientService = clientService;
        _clientView = clientView;
        _inputClient = inputClient; // Ensure _inputClient is assigned
    }


    public override void HandlePresenter()
    {

        ConsoleMenu menu = new ConsoleMenu();
        Account accountSavings = _pseudoDb.accountSavings;
        decimal loanAmount = 1000.0m;
        // Add your menu options
        menu.AddOption("1. View Accounts", DisplayClientAccounts);
        menu.AddOption("2. Transfer Options", () => HandleTransferOption(DisplayTransferOptions()));
        menu.AddOption("3. Open New Account", OpenNewAccount);
        menu.AddOption("4. Get Account By Currency", AccountByCurrency);
        menu.AddOption("5. Request Loan", () => RequestLoan(_pseudoDb.accountSavings,0));
        menu.AddOption("6. Logout", () => Logout());

        menu.AddOption("B. Back", GoBack);


        // Main program loop
        do
        {
            Console.Clear();
            menu.ShowMenu();
            string adminChoice = InputUtils.GetNonEmptyString("\n[Type the option number]"); // Get user's input

            if (string.IsNullOrEmpty(adminChoice))
            {
                Console.WriteLine("Invalid choice. Please try again.");
                continue;
            }

            switch (adminChoice.ToUpper())
            {
                case "1":
                    DisplayClientAccounts();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                case "2":
                    TransferOption selectedTransferOption = DisplayTransferOptions();
                    HandleTransferOption(selectedTransferOption);

                    break;
                case "3":
                    OpenNewAccount();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                case "4":
                    AccountByCurrency();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                    case "5":
                     RequestLoan(_pseudoDb.accountSavings,0);
                    InputUtils.GetNonEmptyString("Press F to exit");

                    break;
                case "6":
                    Logout();
                    break;

                case "B":
                    GoBack(); // Use the GoBack method from the ConsoleMenu
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (menu.IsProgramRunning() && !_logoutRequested);
    }    // Define the GoBack method
    public void GoBack()
    {
        HandlePresenter();
    }
    public void Logout()
    {
        _logoutRequested = true;

    }
    public void DisplayClientAccounts()
    {
        // Get the client whose accounts need to be displayed
        Client client = _pseudoDb.Users.OfType<Client>().FirstOrDefault();

        // Get the updated list of client accounts
        clientAccounts = _clientService.GetClientAccounts(client);

        // Display the client accounts
        _clientView.DisplayClientAccounts(clientAccounts);



         Console.WriteLine();
        var updateChoice = _inputClient.GetYesNoAnswer("Do you want to update your password or email? (Y/N)");

        if (updateChoice )
        {
            UpdateClientCredentials(client);
        }
    }


    private void UpdateClientCredentials(Client client)
    {
        Console.WriteLine("Choose an option to update:");
        Console.WriteLine("1. Update Password");
        Console.WriteLine("2. Update Email");

        var updateOption = Console.ReadLine().Trim();

        switch (updateOption)
        {
            case "1":
                var newPassword = InputUtils.GetNonEmptyString("Enter new password:");
                _pseudoDb.UpdateClientPassword(client, newPassword);
                Console.WriteLine("Password updated successfully!");
                break;
            case "2":
                var newEmail = InputUtils.GetNonEmptyString("Enter new email:");
                _pseudoDb.UpdateClientEmail(client, newEmail);
                Console.WriteLine("Email updated successfully!");
                break;
            default:
                Console.WriteLine("Invalid option. No changes made.");
                break;
        }
    }


    //....................................................................................................................
    public TransferOption DisplayTransferOptions()
    {
        int accountNumberToDisplay = 1; // Update here

        ConsoleMenu menu = new ConsoleMenu();

        // Add your menu options
        menu.AddOption("1. Transfer Money", () => TransferMoney());
        menu.AddOption("2. View transfer history", () => ViewTransferHistory(accountNumberToDisplay));

        // Show the menu
        menu.ShowMenu();

        // Get user input
        string transferOptionString = InputUtils.GetNonEmptyString("\n[Type the option number]");

        TransferOption transferOption;
        if (!Enum.TryParse(transferOptionString, out transferOption))
        {
            Console.WriteLine("Invalid choice. Please select a valid option.");
            return TransferOption.Invalid;
        }

        // Handle the chosen transfer option
        switch (transferOption)
        {
            case TransferOption.TransferBetweenYourAccounts:
                TransferMoney();
                InputUtils.GetNonEmptyString("Press F to exit");
                GoBack();
                break;
            case TransferOption.ViewTransferHistory:
                ViewTransferHistory(accountNumberToDisplay);
                InputUtils.GetNonEmptyString("Press F to exit");
                GoBack();

                break;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");

                break;
        }

        // Return the chosen transfer option
        return transferOption;
    }

    public void HandleTransferOption(TransferOption transferOption)
    {
        switch (transferOption)
        {
            case TransferOption.TransferBetweenYourAccounts:
                TransferMoney();
                break;
            case TransferOption.ViewTransferHistory:
                int accountNumberToDisplay = 1; // Update here with the actual account number to display
                ViewTransferHistory(accountNumberToDisplay);
                break;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                break;
        }
    }
    public void TransferMoney()
    {
        // Prompt the user for the necessary information
        DisplayClientAccounts();
        int sourceAccountNumber = _inputClient.GetAccountNumber("Enter the source account number");
        bool isExternalTransfer = _inputClient.GetYesNoAnswer("Is this an external transfer?");
        int targetAccountNumber = isExternalTransfer
            ? _inputClient.GetAccountNumber("Enter the target account number for the external transfer")
            : _inputClient.GetAccountNumber("Enter the target account number for the internal transfer");
        decimal transferAmount = _inputClient.GetTransferAmount();

        // Perform the transfer
        decimal remainingBalance;
        bool transferSuccess;

        if (isExternalTransfer)
        {
            transferSuccess = _clientService.Transfer(sourceAccountNumber, transferAmount, targetAccountNumber, isExternalTransfer, out remainingBalance);
        }
        else
        {
            transferSuccess = _clientService.Transfer(sourceAccountNumber, transferAmount, targetAccountNumber, false, out remainingBalance);
        }

        // Handle the transfer result
        if (transferSuccess)
        {
            // Add the transfer record to the transfer history
            transferRecords.Add(new TransferRecord
            {
                Timestamp = DateTime.Now,
                SourceAccountNumber = sourceAccountNumber,
                TargetAccountNumber = targetAccountNumber,
                Amount = transferAmount,
                TransferSuccessful = true
            });
            // Display the transfer details
            Console.WriteLine("Transfer Details:");
            Console.WriteLine($"Source Account: {sourceAccountNumber} Target Account: {targetAccountNumber} Amount: {transferAmount} Success: true");
            Console.WriteLine($"Remaining balance in the source account: {remainingBalance:C}");
            Console.WriteLine($"Transfer date and time: {DateTime.Now}");
        }
        else
        {
            Console.WriteLine("Transfer failed. Please check your input and try again.");
        }
    }
    public void ViewTransferHistory(int accountNumber)
    {
        // Get the transfer history for the specified account
        var transferHistory = _clientService.GetTransferHistory(accountNumber);

        if (transferHistory.Any())
        {
            // Display the transfer history
            Console.WriteLine($"Transfer History for Account {accountNumber}:");
            foreach (var record in transferHistory)
            {
                Console.WriteLine($"Timestamp: {record.Timestamp}, Amount: {record.Amount}, " +
                                  $"Source Account: {record.SourceAccountNumber}, " +
                                  $"Target Account: {record.TargetAccountNumber}, " +
                                  $"Success: {record.TransferSuccessful}");
            }
        }
        else
        {
            //Console.WriteLine($"No transfer history found for Account {accountNumber}.");
        }
    }

    //.........................................................................................................................

    public void OpenNewAccount()
    {
        // Check if _client is null and create a new instance if needed
        string accountType = _inputClient.GetAccountType();
        CurrencyCode currencyCode = _inputClient.GetCurrencyCode();
        decimal initialDeposit = _inputClient.GetInitialDeposit();

        // Find the existing client based on the username
        Client existingClient = _pseudoDb.Users.FirstOrDefault(c => c.Username == "2") as Client;

        if (existingClient != null)
        {
            // Use the OpenAccount function to create a new account for the existing client
            bool success = _clientService.OpenAccount(existingClient, currencyCode, accountType, initialDeposit);

            // Display the result to the user
            if (success)
            {
                Console.WriteLine("Account opened successfully!");

            }
            else
            {
                Console.WriteLine("Failed to open the account. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Client with the specified username not found.");
        }
    }

    //................................................................................................................

    public void AccountByCurrency()
    {
        DisplayClientAccounts();

        int targetAccountNumber = _inputClient.GetAccountNumber("Enter the source account number");
        CurrencyCode targetCurrency = _inputClient.GetCurrencyCode();
        decimal targetAccount = _inputClient.GetCurrencyRate();
        decimal amountToConvert = _inputClient.GetTransferAmount();
        Account sourceAccount = _pseudoDb.AllAccounts.FirstOrDefault(account => account.Number == targetAccountNumber);

        Account account = _clientService.AccountByCurrency(targetAccountNumber, targetCurrency);

        if (sourceAccount != null && targetAccountNumber != null)
        {
            decimal convertedAmount = _clientService.ConvertAmount(sourceAccount, targetAccount, amountToConvert);
            Console.WriteLine($"Converted amount: {convertedAmount}");
        }
        else
        {
            Console.WriteLine("Invalid account number or currency.");
        }
        

    }


    //........................

    public bool RequestLoan(Account account, decimal loanAmount)
    {
        loanAmount = _inputClient.GetLoanAmount();
        if (account != null)
        {
            bool loanApproved = _clientService.RequestLoan(account, loanAmount);

            if (loanApproved)
            {
                // Loan approved, perform necessary actions
                Console.WriteLine($"Loan approved for Account ID: {account.Number}");
               
                return true;
            }
            else
            {
                // Loan not approved, handle accordingly
                Console.WriteLine($"Loan not approved for Account ID: {account.Number}");
                return false;
            }
        }
        else
        {
            // Account not found, handle accordingly
            Console.WriteLine("Account not found");
            return false;
        }
    }





}




