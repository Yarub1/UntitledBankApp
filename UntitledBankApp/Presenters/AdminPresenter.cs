using System.Linq.Expressions;
using UntitledBankApp.Models;
using UntitledBankApp.Services;
using UntitledBankApp.Views.Utilities;
namespace UntitledBankApp.Presenters;

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

            menu.AddOption("1. Create User", CreateUser);
            menu.AddOption("2. Set Currency Rate", CurrencyCode);
            menu.AddOption("3. Borrowing Limit", SetBorrowingLimit);
            menu.AddOption("4. Logout", () => Logout());
            menu.AddOption("B. Back", GoBack);

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
                    CreateUser();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                case "2":
                    CurrencyCode();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                case "3":
                    SetBorrowingLimit();
                    InputUtils.GetNonEmptyString("Press F to exit");
                    break;
                case "4":
                    Logout();
                    break;
                case "B":
                    GoBack();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
            }

            if (_menuStack.Count > 0)
            {
                var previousAction = _menuStack.Pop();
                Console.Clear();
                previousAction.Invoke();
                InputUtils.GetNonEmptyString("Press F to exit");
            }
            else
            {
                Console.Clear();
                //Console.WriteLine("Cannot go back any further.");
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
                Console.WriteLine("User created successfully!");
            }
            else
            {
                Console.WriteLine("Failed to create user. Please check the error messages.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
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
             Console.WriteLine($"Exchange rate for {currencyCode} updated to {rate}");
        }
        catch (KeyNotFoundException ex)
        {
            // Handle the case when the specified currency is not found
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
             Console.WriteLine($"An error occurred: {ex.Message}");
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

             Console.WriteLine($"Borrowing limit for {username} updated to {limit}");
        }
        catch (KeyNotFoundException ex)
        {
             Console.WriteLine($"An error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
             Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public void DisplayCustomerList(IEnumerable<User> Users)
    {
        Console.WriteLine("Customer List:");
        foreach (var client in Users)
        {
            Console.WriteLine($"Username: {client.Username}, Full Name: {client.FullName}");
        }
    }
}