global using UntitledBankApp.Models;
global using UntitledBankApp.Presenters;
global using UntitledBankApp.Services;
global using UntitledBankApp.Views;
namespace UntitledBankApp.Views.Utilities;
public static class InputUtils
{
    public static string GetNonEmptyString(string prompt)
    {
        while (true)
        {

            Console.Write($"{char.ToUpper(prompt[0]) + prompt[1..].ToLower()}: ");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                Console.ResetColor();
                return input;
            }

            Console.WriteLine("The input cannot be empty!");


        }

    }
}