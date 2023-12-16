using System;
using System.Linq.Expressions;
using UntitledBankApp.Models;
using UntitledBankApp.Services;
using UntitledBankApp.Views.Utilities;

namespace UntitledBankApp.Presenters
{
    public class AdminPresenter : Presenter
    {
        private PseudoDb _pseudoDb;
        private AdminService _adminService;
        private AdminView _adminView;
        private Dictionary<string, Action> _menuOptions;
        private Stack<Action> _menuStack;
        private bool _logoutRequested = false;

        public AdminPresenter(PseudoDb pseudoDb, AdminService adminService, AdminView adminView)
        {
            _pseudoDb = pseudoDb;
            _adminService = adminService;
            _adminView = adminView;
            _menuStack = new Stack<Action>();
        }

        public override void HandlePresenter()
        {
            do
            {
                Console.Clear();
                ConsoleMenu menu = new ConsoleMenu();
                menu.ShowMenu();

                menu.AddOption($"1. Create User", CreateUser);
                menu.AddOption($"2. Set Currency Rate", CurrencyCode);
                menu.AddOption($"3. Borrowing Limit", SetBorrowingLimit);
                menu.AddOption($"4. Logout", () => Logout());
                menu.AddOption($"B. Back", GoBack);

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
                        CreateUser();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "2":
                        CurrencyCode();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "3":
                        SetBorrowingLimit();
                        InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                        break;
                    case "4":
                        Logout();
                        break;
                    case "B":
                        GoBack();
                        break;
                    default:
                        Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please try again.{ConsoleColors.Reset}");
                        continue;
                }

                if (_menuStack.Count > 0)
                {
                    var previousAction = _menuStack.Pop();
                    Console.Clear();
                    previousAction.Invoke();
                    InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Press F to exit{ConsoleColors.Reset}");
                }
                else
                {
                    Console.Clear();
                    //Console.WriteLine($"{ConsoleColors.Red}Cannot go back any further.{ConsoleColors.Reset}");
                }
            } while (!_logoutRequested);
        }

        // Define the GoBack method
        private void GoBack()
        {
            HandlePresenter();
        }

        public void Logout()
        {
            _logoutRequested = true;
        }

        public enum UserType
        {
            Admin,
            Client
        }

        public void CreateUser()
        {
            var createUserResult = _adminView.CreateUser();

            try
            {
                Role userRole = createUserResult.userType == UserType.Admin ? Role.Admin : Role.Client;
                bool userCreated = _adminService.CreateUser(userRole, createUserResult.fullName, createUserResult.username, createUserResult.password, createUserResult.passwordVerified, createUserResult.email, createUserResult.emailVerified, createUserResult.address, createUserResult.telephonenumber);

                if (userCreated)
                {
                    Console.WriteLine($"{ConsoleColors.Green}User created successfully!{ConsoleColors.Reset}");
                }
                else
                {
                    Console.WriteLine($"{ConsoleColors.Red}Failed to create user. Please check the error messages.{ConsoleColors.Reset}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ConsoleColors.Red}An error occurred: {ex.Message}{ConsoleColors.Reset}");
            }
        }

        public void CurrencyCode()
        {
            try
            {
                // Get currency code and rate from the admin view
                var currencyCode = _adminView.GetCurrencyCode();
                var rate = _adminView.GetExchangeRate();

                // Set the currency rate
                _adminService.SetCurrencyRate(currencyCode, rate);

                // Notify the user and press any key to continue
                Console.WriteLine($"{ConsoleColors.Cyan}Exchange rate for {currencyCode} updated to {rate}{ConsoleColors.Reset}");
            }
            catch (KeyNotFoundException ex)
            {
                // Handle the case when the specified currency is not found
                Console.WriteLine($"{ConsoleColors.Red}{ex.Message}{ConsoleColors.Reset}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"{ConsoleColors.Red}An error occurred: {ex.Message}{ConsoleColors.Reset}");
            }
        }

        public void SetBorrowingLimit()
        {
            try
            {
                var clients = _adminService.GetCustomerList();
                var username = _adminView.ChooseClientFromList(clients);
                var limit = _adminView.GetBorrowingLimit();

                _adminService.SetBorrowingLimit(username, limit);

                Console.WriteLine($"{ConsoleColors.Cyan}Borrowing limit for {username} updated to {limit}{ConsoleColors.Reset}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"{ConsoleColors.Red}An error occurred: {ex.Message}{ConsoleColors.Reset}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ConsoleColors.Red}An error occurred: {ex.Message}{ConsoleColors.Reset}");
            }
        }

        public void DisplayCustomerList(IEnumerable<User> Users)
        {
            Console.WriteLine($"{ConsoleColors.Cyan}Customer List:{ConsoleColors.Reset}");
            foreach (var client in Users)
            {
                Console.WriteLine($"{ConsoleColors.Magenta}Username: {client.Username}, Full Name: {client.FullName}{ConsoleColors.Reset}");
            }
        }
    }
}
