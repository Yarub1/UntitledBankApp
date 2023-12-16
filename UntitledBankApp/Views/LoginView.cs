using UntitledBankApp.Views.Utilities;

public class LoginView : View
{
    public (string username, string password) GetCredentials()
    {
        Console.WriteLine($"{ConsoleColors.Cyan}Welcome to the Banking App{ConsoleColors.Reset}");
        var menuOptions = new string[] { $"{ConsoleColors.Green}1. Login{ConsoleColors.Reset}", $"{ConsoleColors.Yellow}2. About{ConsoleColors.Reset}", $"{ConsoleColors.Red}3. Exit{ConsoleColors.Reset}" };
        Array.ForEach(menuOptions, Console.WriteLine);

        bool Choices = false;
        while (!Choices)
        {
            string LoginChoice = InputUtils.GetNonEmptyString($"{ConsoleColors.Magenta}[Type Option Number]{ConsoleColors.Reset}"); // Get user's input

            // Perform the selected action
            switch (LoginChoice)
            {
                case "1":
                    Console.Clear();
                    var username = InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Username{ConsoleColors.Reset}");
                    var password = GetHiddenPassword($"{ConsoleColors.Yellow}Password{ConsoleColors.Reset}:");
                    return (username, password);
                case "2":
                    DisplayAbout();
                    break;
                case "3":
                    Environment.Exit(0); // Exit the application
                    break;
                case "4":
                    Choices = true; // Exit the loop to indicate logout
                    break;
                default:
                    Console.WriteLine($"{ConsoleColors.Red}Invalid choice. Please try again.{ConsoleColors.Reset}");
                    break;
            }
        }
        return (null, null);
    }

    protected override void DisplayHeader()
    {
        // Header logic
    }

    private void DisplayAbout()
    {
        var aboutNames = new string[] { "Adrian Moreno", "Alexander Doja", "Erik Berglund", "Theodor Hägg", "Yarub Adnan" };

        DisplayAboutNames(aboutNames);
        Console.ResetColor();
    }

    void DisplayAboutNames(string[] names)
    {
        foreach (var name in names)
        {
            Console.WriteLine($"{ConsoleColors.White}{name}{ConsoleColors.Reset}");
            System.Threading.Thread.Sleep(1000);
        }
        Console.Clear();
        Console.ReadKey();
        GetCredentials();
    }

    private string GetHiddenPassword(string prompt)
    {
        Console.Write($"{ConsoleColors.Yellow}{prompt}{ConsoleColors.Reset}");
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignore any key that is not a printable ASCII character
            if (key.Key != ConsoleKey.Backspace && key.KeyChar != '\u0000' && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write($"{ConsoleColors.Green}*{ConsoleColors.Reset}");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write($"{ConsoleColors.Red}\b \b{ConsoleColors.Reset}");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after Enter is pressed
        return password;
    }
}