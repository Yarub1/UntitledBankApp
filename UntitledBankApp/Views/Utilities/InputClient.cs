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
        /// <summary>
        /// This method prompts the user with a yes/no question and returns a boolean value based on the user's response.
        /// </summary>
        /// <param name="prompt">The question to ask the user.</param>
        /// <returns>True if the user answers 'yes' or 'y', False if the user answers 'no' or 'n'.</returns>
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

        /// <summary>
        /// This method prompts the user to enter an account number and returns the entered value as an integer.
        /// </summary>
        /// <param /*name="prompt*/">The prompt to display to the user.</param>
        /// <returns>The entered account number as an integer.</returns>
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

        /// <summary>
        /// This method prompts the user to enter a transfer amount and returns the entered value as a decimal.
        /// </summary>
        /// <returns>The entered transfer amount as a decimal.</returns>
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

        /// <summary>
        /// This method prompts the user to enter an account type (savings or checking) and returns the entered value as a string.
        /// </summary>
        /// <returns>The entered account type as a string.</returns>
        public string GetAccountType()
        {
            return InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the account type (savings, checking){ConsoleColors.Reset}");
        }

        /// <summary>
        /// This method prompts the user to enter a currency code (SEK, USD, EUR) and returns the entered value as a CurrencyCode enum.
        /// </summary>
        /// <returns>The entered currency code as a CurrencyCode enum.</returns>
        public CurrencyCode GetCurrencyCode()
        {
            string currencyCodeStr = InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the currency code (SEK, USD, EUR){ConsoleColors.Reset}".ToUpper());
            return (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currencyCodeStr, true);
        }

        /// <summary>
        /// This method prompts the user to enter an initial deposit amount and returns the entered value as a decimal.
        /// </summary>
        /// <returns>The entered initial deposit amount as a decimal.</returns>
        public decimal GetInitialDeposit()
        {
            decimal initialDeposit;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the initial deposit amount{ConsoleColors.Reset}"), out initialDeposit))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
            return initialDeposit;
        }

        /// <summary>
        /// This method prompts the user to enter a currency rate and returns the entered value as a decimal.
        /// </summary>
        /// <returns>The entered currency rate as a decimal.</returns>
        public decimal GetCurrencyRate()
        {
            decimal CurrencyRate;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString($"{ConsoleColors.White}Enter the currency rate{ConsoleColors.Reset}"), out CurrencyRate))
            {
                Console.WriteLine($"{ConsoleColors.Red}Invalid input. Please enter a valid decimal number.{ConsoleColors.Reset}");
            }
            return CurrencyRate;
        }

        /// <summary>
        /// This method prompts the user to enter a loan amount and returns the entered value as a decimal.
        /// </summary>
        /// <returns>The entered loan amount as a decimal.</returns>
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
