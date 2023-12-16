using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UntitledBankApp.Presenters.AdminPresenter;

namespace UntitledBankApp.Views.Utilities
{
    public class InputClient
    {
        public bool GetYesNoAnswer(string prompt)
        {
            while (true)
            {
                string answer = InputUtils.GetNonEmptyString($"{ConsoleColors.White}{prompt} (yes/no){ConsoleColors.Reset}");

                if (answer == "yes" || answer == "y")
                {
                    return true;
                }
                else if (answer == "no" || answer == "n")
                {
                    return false;
                }

                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter 'yes' or 'no'.{ConsoleColors.Reset}");
            }
        }

        public int GetAccountNumber(string prompt)
        {
            while (true)
            {
                var input = InputUtils.GetNonEmptyString($"{ConsoleColors.White}{prompt}{ConsoleColors.Reset}");

                if (int.TryParse(input, out int accountNumber) && !string.IsNullOrWhiteSpace(input))
                {
                    Console.ResetColor();
                    return accountNumber;
                }

                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid account number.{ConsoleColors.Reset}");
            }
        }

        public decimal GetTransferAmount()
        {
            while (true)
            {
                var input = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the transfer amount{ConsoleColors.Reset}");

                if (decimal.TryParse(input, out decimal transferAmount) && !string.IsNullOrWhiteSpace(input))
                {
                    Console.ResetColor();
                    return transferAmount;
                }

                Console.WriteLine($"{ConsoleColors.Red}Invalid transfer amount format. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
        }

        public string GetAccountType()
        {
            return InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the account type (savings, checking){ConsoleColors.Reset}");
        }

        public CurrencyCode GetCurrencyCode()
        {
            string currencyCodeStr = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the currency code (SEK, USD, EUR){ConsoleColors.Reset}".ToUpper());
            return (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currencyCodeStr, true);
        }

        public decimal GetInitialDeposit()
        {
            decimal initialDeposit;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the initial deposit amount{ConsoleColors.Reset}"), out initialDeposit))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
            return initialDeposit;
        }

        public decimal GetCurrencyRate()
        {
            decimal CurrencyRate;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the currency rate{ConsoleColors.Reset}"), out CurrencyRate))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
            return CurrencyRate;
        }

        public decimal GetLoanAmount()
        {
            decimal loanAmount;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the loan Amount{ConsoleColors.Reset}"), out loanAmount))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
            return loanAmount;
        }
    }
}
