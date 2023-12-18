using UntitledBankApp.Factories;
using UntitledBankApp.Views.Utilities;
using UntitledBankApp;
using System;

/// <summary>
/// The LoginPresenter class handles the logic for the login functionality.
/// </summary>
public class LoginPresenter : Presenter
{
    private PseudoDb _pseudoDb;
    private LoginService _loginService;
    private LoginView _loginView;
    private const int MaxLoginAttempts = 3;
    private int _loginAttempts;
    private bool _logoutRequested = false;

    public LoginPresenter(PseudoDb pseudoDb, LoginService loginService, LoginView loginView)
    {
        _pseudoDb = pseudoDb;
        _loginService = loginService;
        _loginView = loginView;
        _loginAttempts = 0;
    }

    /// <summary>
    /// Handles the presenter logic.
    /// </summary>
    public override void HandlePresenter()
    {
        while (!_logoutRequested)
        {
            // Reset login attempts for each login attempt
            _loginAttempts = 0;

            // Allow multiple login attempts until logout is requested
            while (_loginAttempts < MaxLoginAttempts)
            {
                var user = GetUserFromCredentials();

                if (user != null)
                {
                    RedirectUserBasedOnRole(user);
                    break; // Exit the login attempt loop on successful login
                }

                _loginAttempts++;
                Console.WriteLine($"{ConsoleColors.Red}Invalid login attempt {_loginAttempts}/{MaxLoginAttempts}. Please try again.{ConsoleColors.Reset}");
            }

            // Display a message or take action after reaching the maximum login attempts
            if (_loginAttempts == MaxLoginAttempts)
            {
                Console.WriteLine($"{ConsoleColors.Red}Maximum login attempts reached. Exiting application.{ConsoleColors.Reset}");
                Environment.Exit(0);
            }
        }
    }

    /// <summary>
    /// Retrieves the user from the provided credentials.
    /// </summary>
    /// <returns>The user object if the credentials are valid, otherwise null.</returns>
    private User? GetUserFromCredentials()
    {
        var (username, password, captcha) = _loginView.GetCredentials();
        var user = _loginService.GetUser(username, password, captcha);

        return user;
    }

    /// <summary>
    /// Redirects the user based on their role.
    /// </summary>
    /// <param name="user">The user object.</param>
    private void RedirectUserBasedOnRole(User user)
    {
        switch (user)
        {
            case Client client:
                RunPresenter(new ClientPresenter(_pseudoDb, new ClientService(_pseudoDb), new ClientView(client), new InputClient()));
                break;
            case Admin admin:
                RunPresenter(new AdminPresenter(_pseudoDb, new AdminService(_pseudoDb, new UserFactory()), new AdminView(admin)));
                break;
        }
    }

    /// <summary>
    /// Requests a logout from the presenter.
    /// </summary>
    public void RequestLogout()
    {
        _logoutRequested = true;
        Console.WriteLine($"{ConsoleColors.Cyan}Logging out...{ConsoleColors.Reset}");
    }
}
