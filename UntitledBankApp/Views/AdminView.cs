using System;
using UntitledBankApp;
using UntitledBankApp.Views.Utilities;
using static UntitledBankApp.Presenters.AdminPresenter;

namespace UntitledBankApp.Views
{
    public class AdminView : View
    {
        private Admin _admin;

        public AdminView(Admin admin)
        {
            _admin = admin;
        }

        protected override void DisplayHeader()
        {
            Console.WriteLine($"{ConsoleColors.Cyan}LOGGED IN AS ADMIN {_admin.Username}{ConsoleColors.Reset}");
        }

        public (UserType userType, string fullName, string username, string password, string passwordVerified, string email, string emailVerified, string address, string telephonenumber) CreateUser()
        {
            int userTypeInput = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                string userTypeInputString = InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Select user type:\n1. Admin\n2. Client\nEnter the user type (1 or 2): {ConsoleColors.Reset}");

                isValidInput = int.TryParse(userTypeInputString, out userTypeInput);

                if (!isValidInput || (userTypeInput != 1 && userTypeInput != 2))
                {
                    Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter 1 or 2 for the user type.{ConsoleColors.Reset}");
                    isValidInput = false;
                }
            }

            UserType userType = userTypeInput == 1 ? UserType.Admin : UserType.Client;

            string fullName = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Full Name{ConsoleColors.Reset}");
            string username = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Username{ConsoleColors.Reset}");
            string password = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Password{ConsoleColors.Reset}");
            string passwordVerified = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Verify Password{ConsoleColors.Reset}");
            string email = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Email{ConsoleColors.Reset}");
            string emailVerified = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Verify Email{ConsoleColors.Reset}");
            string address = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Address{ConsoleColors.Reset}");
            string telephonenumber = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Telephone Number{ConsoleColors.Reset}");

            return (userType, fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber);
        }

        public CurrencyCode GetCurrencyCode()
        {
            return Enum.Parse<CurrencyCode>(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the currency code to update (USD, EUR, SEK){ConsoleColors.Reset}").ToUpper());
        }

        public decimal GetExchangeRate()
        {
            return decimal.Parse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the new exchange rate{ConsoleColors.Reset}"));
        }

        public decimal GetBorrowingLimit()
        {
            return decimal.Parse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter new borrowing limit{ConsoleColors.Reset}"));
        }

        public void DisplayCustomerList(IEnumerable<Client> clients)
        {
            Console.WriteLine($"{ConsoleColors.Yellow}Customer List:{ConsoleColors.Reset}");
            foreach (var client in clients)
            {
                Console.WriteLine($"{ConsoleColors.White}Username: {client.Username}, Full Name: {client.FullName}{ConsoleColors.Reset}");
            }
        }

        public string ChooseClientFromList(IEnumerable<Client> clients)
        {
            Console.WriteLine($"{ConsoleColors.Black}Choose a client from the list:{ConsoleColors.Reset}");
            DisplayCustomerList(clients);
            return InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the username of the client{ConsoleColors.Reset}");
        }
    }
}
