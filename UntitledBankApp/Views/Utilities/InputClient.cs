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

                string answer = InputUtils.GetNonEmptyString($"{prompt} (yes/no)");

                if (answer == "yes" || answer == "y")
                {
                    return true;
                }
                else if (answer == "no" || answer == "n")
                {
                    return false;
                }

                Console.WriteLine ("Invalid input. Please enter 'yes' or 'no'.");
            }
        }



        public int GetAccountNumber(string prompt)
        {
            //Console.WriteLine (prompt);

            while (true)
            {
                var input = InputUtils.GetNonEmptyString($"{prompt}");

                if (int.TryParse(input, out int accountNumber) && !string.IsNullOrWhiteSpace(input))
                {
                    Console.ResetColor();
                    return accountNumber;
                }

                Console.WriteLine("Invalid input. Please enter a valid account number.");
            }
        }


        public decimal GetTransferAmount()
        {

            while (true)
            {
                var input = InputUtils.GetNonEmptyString($"Enter the transfer amount");

                if (decimal.TryParse(input, out decimal transferAmount) && !string.IsNullOrWhiteSpace(input))
                {
                    Console.ResetColor();
                    return transferAmount;
                }

                Console.WriteLine ("Invalid transfer amount format. Please enter a valid decimal number.");
            }
        }









        public string GetAccountType()
        {
            return InputUtils.GetNonEmptyString("Enter the account type (savings, checking)");
        }

        public CurrencyCode GetCurrencyCode()
        {
            // Add logic to convert the user input to CurrencyCode
            // For example, you might parse the string or use a dictionary to map input to CurrencyCode
            string currencyCodeStr = InputUtils.GetNonEmptyString("Enter the currency code (SEK, USD, EUR)".ToUpper());
            // Assume CurrencyCode is an enum
            return (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currencyCodeStr, true);

        }

        public decimal GetInitialDeposit()
        {
            // Add logic to validate and parse the user input as a decimal
            decimal initialDeposit;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString("Enter the initial deposit amount"), out initialDeposit))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                Console.WriteLine("Enter the initial deposit amount: ");
            }
            return initialDeposit;
        }


        public decimal GetCurrencyRate()
        {
            // Add logic to validate and parse the user input as a decimal
            decimal CurrencyRate;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString("Enter the currency rate"), out CurrencyRate))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            }
            return CurrencyRate;
        }

        public decimal GetLoanAmount()
        {
            // Add logic to validate and parse the user input as a decimal
            decimal loanAmount;
            while (!decimal.TryParse(InputUtils.GetNonEmptyString("Enter the loan Amount"), out loanAmount))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            }
            return loanAmount;
        }


    }
}
