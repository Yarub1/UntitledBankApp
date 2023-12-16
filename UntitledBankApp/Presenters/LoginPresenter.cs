using UntitledBankApp.Factories;
using UntitledBankApp.Views.Utilities;
using UntitledBankApp;


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
                    Console.WriteLine($"Invalid login attempt {_loginAttempts}/{MaxLoginAttempts}. Please try again.");
                }

                // Display a message or take action after reaching the maximum login attempts
                if (_loginAttempts == MaxLoginAttempts)
                {
                    Console.WriteLine("Maximum login attempts reached. Exiting application.");
                    Environment.Exit(0);
                }
            }
        }

        private User? GetUserFromCredentials()
        {
            var (username, password) = _loginView.GetCredentials();
            var user = _loginService.GetUser(username, password);

            return user;
        }

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

        public void RequestLogout()
        {
            _logoutRequested = true;
            Console.WriteLine("Logging out...");
        }
    }

