using System;
using System.Security.Principal;
using UntitledBankApp.Models;
using UntitledBankApp.Services;
using UntitledBankApp.Views;
using UntitledBankApp.Views.Utilities;
using static ClientView;

namespace UntitledBankApp.Presenters
{
    public class ClientPresenter : Presenter
    {
        // This section declares private fields and initializes them with default values.
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
        // Constructor for the ClientPresenter class
        {
            _pseudoDb = pseudoDb;
            _clientService = clientService;
            _clientView = clientView;
            _inputClient = inputClient; // Ensure _inputClient is assigned
        }
        /// <summary>
        /// This method handles the presenter logic for the client. It displays a menu with various options for the client to choose from, such as viewing accounts, transferring money, opening new accounts, and requesting loans. It also handles the user's input and performs the corresponding actions based on the selected option.
        /// </summary>
        public override void HandlePresenter()
        {
            ConsoleMenu menu = new ConsoleMenu();
            Account accountSavings = _pseudoDb.accountSavings;
            decimal loanAmount = 1000.0m;

            // Add your menu options
            menu.AddOption($"1. View Accounts", DisplayClientAccountsWithUpdate);
            menu.AddOption($"2. Transfer Options", () => HandleTransferOption(DisplayTransferOptions()));
            menu.AddOption($"3. Open New Account", OpenNewAccount);
            menu.AddOption($"4. Get Account By Currency", AccountByCurrency);
            menu.AddOption($"5. Request Loan", () => RequestLoan(_pseudoDb.accountSavings, 0));
            menu.AddOption($"{ConsoleColors.Red}6. Logout{ConsoleColors.Reset}", () => Logout());

            menu.AddOption($"{ConsoleColors.White}B. Back{ConsoleColors.Reset}", GoBack);

            // Main program loop
            do
            {
                Console.Clear();
                menu.ShowMenu();
                string adminChoice = InputUtils.GetNonEmptyString($"\n{ConsoleColors.White}[Type the option number]{ConsoleColors.Reset}"); // Get user's input

                if (string.IsNullOrEmpty(adminChoice))
                {
                    Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please try again.{ConsoleColors.Reset}");
                    continue;
                }

                switch (adminChoice.ToUpper())
                {
                    case "1":
                        DisplayClientAccountsWithUpdate();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "2":
                        TransferOption selectedTransferOption = DisplayTransferOptions();
                        HandleTransferOption(selectedTransferOption);
                        break;
                    case "3":
                        OpenNewAccount();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "4":
                        AccountByCurrency();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "5":
                        RequestLoan(_pseudoDb.accountSavings, 0);
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "6":
                        Logout();
                        break;
                    case "B":
                        GoBack(); // Use the GoBack method from the ConsoleMenu
                        break;
                    default:
                        Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please try again.{ConsoleColors.Reset}");
                        break;
                }
            } while (menu.IsProgramRunning() && !_logoutRequested);
        }

        public void GoBack()
        {
            HandlePresenter();
        }

        public void Logout()
        {
            _logoutRequested = true;
        }

        /// <summary>
        /// Displays the client's accounts and allows for updating the password or email.
        /// </summary>
        public void DisplayClientAccountsWithUpdate()
        {
            // Get the client whose accounts need to be displayed
            Client client = _pseudoDb.Users.OfType<Client>().FirstOrDefault();

            // Get the updated list of client accounts
            clientAccounts = _clientService.GetClientAccounts(client);

            // Display the client accounts
            _clientView.DisplayClientAccounts(clientAccounts);

            InputClient _inputClient = new InputClient();

            Console.WriteLine();
            var updateChoice = _inputClient.GetYesNoAnswer($"{ConsoleColors.Yellow}Do you want to update your password or email? (Y/N){ConsoleColors.Reset}");

            if (updateChoice)
            {
                UpdateClientCredentials(client);
            }
        }

        /// <summary>
        /// This method is responsible for displaying the client's accounts.
        /// It retrieves the client from the pseudo database and gets the updated list of client accounts using the client service.
        /// Finally, it calls the client view to display the client accounts.
        /// </summary>
        public void DisplayClientAccounts()
        {
            // Get the client whose accounts need to be displayed
            Client client = _pseudoDb.Users.OfType<Client>().FirstOrDefault();

            // Get the updated list of client accounts
            clientAccounts = _clientService.GetClientAccounts(client);

            // Display the client accounts
            _clientView.DisplayClientAccounts(clientAccounts);
        }

        /// <summary>
        /// Updates the client's credentials (password or email) based on the selected option.
        /// </summary>
        /// <param name="client">The client whose credentials need to be updated.</param>
        public void UpdateClientCredentials(Client client)
        {
            Console.WriteLine($"{ConsoleColors.Cyan}Choose an option to update:{ConsoleColors.Reset}");
            Console.WriteLine($"1. Update Password");
            Console.WriteLine($"2. Update Email");

            var updateOption = Console.ReadLine().Trim();

            switch (updateOption)
            {
                case "1":
                    var newPassword = InputUtils.GetNonEmptyString($"{ConsoleColors.Cyan}Enter new password:{ConsoleColors.Reset}");
                    _pseudoDb.UpdateClientPassword(client, newPassword);
                    Console.WriteLine($"{ConsoleColors.Green}Password updated successfully!{ConsoleColors.Reset}");
                    break;
                case "2":
                    var newEmail = InputUtils.GetNonEmptyString($"{ConsoleColors.Cyan}Enter new email:{ConsoleColors.Reset}");
                    _pseudoDb.UpdateClientEmail(client, newEmail);
                    Console.WriteLine($"{ConsoleColors.Green}Email updated successfully!{ConsoleColors.Reset}");
                    break;
                default:
                    Console.WriteLine($"{ConsoleColors.Red}Invalid option. No changes made.{ConsoleColors.Reset}");
                    break;
            }
        }

        public TransferOption DisplayTransferOptions()
        // This method displays the transfer options to the client and allows them to choose an option.
        // It first prompts the client to enter the option number.
        // If the entered option is invalid, it displays an error message and returns TransferOption.Invalid.
        // If the entered option is TransferBetweenYourAccounts, it calls the TransferMoney() method and then prompts the client to press F to exit and goes back to the previous menu.
        // If the entered option is ViewTransferHistory, it calls the ViewTransferHistory() method and then prompts the client to press F to exit and goes back to the previous menu.
        // If the entered option is any other value, it displays an error message.
        {
            int accountNumberToDisplay = 1; // Update here

            ConsoleMenu menu = new ConsoleMenu();

            menu.AddOption($"{ConsoleColors.Cyan}1. Transfer Money{ConsoleColors.Reset}", () => TransferMoney());
            menu.AddOption($"{ConsoleColors.Cyan}2. View transfer history{ConsoleColors.Reset}", () => ViewTransferHistory(accountNumberToDisplay));

            menu.ShowMenu();

            string transferOptionString = InputUtils.GetNonEmptyString($"{ConsoleColors.White}[Type the option number]{ConsoleColors.Reset}");

            TransferOption transferOption;
            if (!Enum.TryParse(transferOptionString, out transferOption))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please select a valid option.{ConsoleColors.Reset}");
                return TransferOption.Invalid;
            }

            switch (transferOption)
            {
                case TransferOption.TransferBetweenYourAccounts:
                    TransferMoney();
                    InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                    GoBack();
                    break;
                case TransferOption.ViewTransferHistory:
                    ViewTransferHistory(accountNumberToDisplay);
                    InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                    GoBack();
                    break;
                default:
                    Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please select a valid option.{ConsoleColors.Reset}");
                    break;
            }

            return transferOption;
        }

        /// <summary>
        /// Handles the selected transfer option and performs the corresponding action.
        /// </summary>
        /// <param name="transferOption">The selected transfer option.</param>
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
                    Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please select a valid option.{ConsoleColors.Reset}");
                    break;
            }
        }

        /// <summary>
        /// This method transfers money from one account to another.
        /// It prompts the user to enter the source account number, whether it is an external transfer, and the target account number.
        /// It then prompts the user to enter the transfer amount.
        /// The method calls the Transfer method of the ClientService class to perform the transfer.
        /// If the transfer is successful, it adds a new TransferRecord to the transferRecords list and displays the transfer details.
        /// If the transfer fails, it displays an error message.
        /// </summary>
        public void TransferMoney()
        {
            DisplayClientAccounts();
            int sourceAccountNumber = _inputClient.GetAccountNumber($"{ConsoleColors.Cyan}Enter the source account number{ConsoleColors.Reset}");
            bool isExternalTransfer = _inputClient.GetYesNoAnswer($"{ConsoleColors.Cyan}Is this an external transfer?{ConsoleColors.Reset}");
            int targetAccountNumber = isExternalTransfer
                ? _inputClient.GetAccountNumber($"{ConsoleColors.Cyan}Enter the target account number for the external transfer{ConsoleColors.Reset}")
                : _inputClient.GetAccountNumber($"{ConsoleColors.Cyan}Enter the target account number for the internal transfer{ConsoleColors.Reset}");
            decimal transferAmount = _inputClient.GetTransferAmount();

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

            if (transferSuccess)
            {
                transferRecords.Add(new TransferRecord
                {
                    Timestamp = DateTime.Now,
                    SourceAccountNumber = sourceAccountNumber,
                    TargetAccountNumber = targetAccountNumber,
                    Amount = transferAmount,
                    TransferSuccessful = true
                });

                Console.WriteLine($"{ConsoleColors.Green}Transfer Details:{ConsoleColors.Reset}");
                Console.WriteLine($"{ConsoleColors.Magenta}Source Account: {sourceAccountNumber} Target Account: {targetAccountNumber} Amount: {transferAmount} Success: true{ConsoleColors.Reset}");
                Console.WriteLine($"{ConsoleColors.Cyan}Remaining balance in the source account: {remainingBalance:C}{ConsoleColors.Reset}");
                Console.WriteLine($"{ConsoleColors.Cyan}Transfer date and time: {DateTime.Now}{ConsoleColors.Reset}");
            }
            else
            {
                Console.WriteLine($"{ConsoleColors.Red}Transfer failed. Please check your input and try again.{ConsoleColors.Reset}");
            }
        }

        /// <summary>
        /// Displays the transfer history for the specified account number.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        public void ViewTransferHistory(int accountNumber)
        {
            var transferHistory = _clientService.GetTransferHistory(accountNumber);

            if (transferHistory.Any())
            {
                Console.WriteLine($"{ConsoleColors.Cyan}Transfer History for Account {accountNumber}:{ConsoleColors.Reset}");
                foreach (var record in transferHistory)
                {
                    Console.WriteLine($"{ConsoleColors.Magenta}Timestamp: {record.Timestamp}, Amount: {record.Amount}, " +
                                      $"Source Account: {record.SourceAccountNumber}, " +
                                      $"Target Account: {record.TargetAccountNumber}, " +
                                      $"Success: {record.TransferSuccessful}{ConsoleColors.Reset}");
                }
            }
            else
            {
                Console.WriteLine($"{ConsoleColors.Cyan}No transfer history found for Account {accountNumber}.{ConsoleColors.Reset}");
            }
        }

        /// <summary>
        /// Opens a new account for an existing client.
        /// </summary>
        public void OpenNewAccount()
        /// <summary>
        /// This method is responsible for opening a new account for a client.
        /// It prompts the user to enter the account type, currency code, and initial deposit.
        /// It then checks if the client exists in the database and opens the account if successful.
        /// If the account opening is successful, it displays a success message.
        /// If the account opening fails, it displays an error message.
        /// </summary>
        {
            try
            {
                string accountType = _inputClient.GetAccountType();
                CurrencyCode currencyCode = _inputClient.GetCurrencyCode();
                decimal initialDeposit = _inputClient.GetInitialDeposit();

                Client existingClient = _pseudoDb.Users.FirstOrDefault(c => c.Username == "2") as Client;

                if (existingClient != null)
                {
                    bool success = _clientService.OpenAccount(existingClient, currencyCode, accountType, initialDeposit);

                    if (success)
                    {
                        Console.WriteLine($"{ConsoleColors.Green}Account opened successfully!{ConsoleColors.Reset}");
                    }
                    else
                    {
                        Console.WriteLine($"{ConsoleColors.Red}Failed to open the account. Please try again.{ConsoleColors.Reset}");
                    }
                }
                else
                {
                    Console.WriteLine($"{ConsoleColors.Red}Client with the specified username not found.{ConsoleColors.Reset}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ConsoleColors.Red}An error occurred while opening the account: {ex.Message}{ConsoleColors.Reset}");
            }
        }

        public void AccountByCurrency()
        /// <summary>
        /// Retrieves an account based on the specified account number and currency code.
        /// Converts the specified amount from the source account's currency to the target currency.
        /// </summary>
        /// <param name="targetAccountNumber">The target account number.</param>
        /// <param name="targetCurrency">The target currency code.</param>
        /// <param name="targetAccount">The target account's currency rate.</param>
        /// <param name="amountToConvert">The amount to convert.</param>
        /// <returns>The converted amount in the target currency.</returns>
        {
            DisplayClientAccounts();

            int targetAccountNumber = _inputClient.GetAccountNumber($"{ConsoleColors.Cyan}Enter the target account number{ConsoleColors.Reset}");
            CurrencyCode targetCurrency = _inputClient.GetCurrencyCode();
            decimal targetAccount = _inputClient.GetCurrencyRate();
            decimal amountToConvert = _inputClient.GetTransferAmount();
            Account sourceAccount = _pseudoDb.AllAccounts.FirstOrDefault(account => account.Number == targetAccountNumber);

            Account account = _clientService.AccountByCurrency(targetAccountNumber, targetCurrency);

            if (sourceAccount != null && targetAccountNumber != null)
            {
                decimal convertedAmount = _clientService.ConvertAmount(sourceAccount, targetAccount, amountToConvert);
                Console.WriteLine($"{ConsoleColors.Cyan}Converted amount: {convertedAmount}{ConsoleColors.Reset}");
            }
            else
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid account number or currency.{ConsoleColors.Reset}");
            }
        }
           /// <summary>
        /// Requests a loan for the specified account with the given loan amount.
        /// </summary>
        /// <param name="account">The account to request the loan for.</param>
        /// <param name="loanAmount">The amount of the loan.</param>
        /// <returns>True if the loan is approved, false otherwise.</returns>
        public bool RequestLoan(Account account, decimal loanAmount)
        {
            loanAmount = _inputClient.GetLoanAmount();

            if (account != null)
            {
                bool loanApproved = _clientService.RequestLoan(account, loanAmount);

                if (loanApproved)
                {
                    Console.WriteLine($"{ConsoleColors.Green}Loan approved for Account ID: {account.Number}{ConsoleColors.Reset}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"{ConsoleColors.Red}Loan not approved for Account ID: {account.Number}{ConsoleColors.Reset}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"{ConsoleColors.Red}Account not found{ConsoleColors.Reset}");
                return false;
            }
        }
    }
}
