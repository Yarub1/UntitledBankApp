using UntitledBankApp.Views.Utilities;

public class LoginView : View
{
    public (string username, string password) GetCredentials()
    {
        Console.WriteLine("Welcome to the Banking App");
        var menuOptions = new string[] { "1. Login", "2. About", "3. Exit" };
        Array.ForEach(menuOptions, Console.WriteLine);

        bool Choices = false;
        while (!Choices)
        {
            string LoginChoice = InputUtils.GetNonEmptyString("[Type Option Number]"); // Get user's input

            // Perform the selected action
            switch (LoginChoice)
            {
                case "1":
                    Console.Clear();
                    var username = InputUtils.GetNonEmptyString("Username");
                    var password = GetHiddenPassword("Password:");
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
                    Console.WriteLine("Invalid choice. Please try again.");
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
            Console.WriteLine(name);
            System.Threading.Thread.Sleep(1000);
        }
        Console.Clear();
        Console.ReadKey();
        GetCredentials();
    }

    private string GetHiddenPassword(string prompt)
    {
        Console.Write(prompt);
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignore any key that is not a printable ASCII character
            if (key.Key != ConsoleKey.Backspace && key.KeyChar != '\u0000' && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after Enter is pressed
        return password;
    }
}
