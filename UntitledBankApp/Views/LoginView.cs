using UntitledBankApp.Views.Utilities;

/// <summary>
/// This class represents the login view of the Banking App.
/// </summary>
public class LoginView : View
{
    /// <summary>
    /// Gets the user's credentials.
    /// </summary>
    /// <returns>A tuple containing the username, password, and captcha.</returns>
    public (string username, string password, string captcha) GetCredentials()
    {
        Console.WriteLine($"{ConsoleColors.Cyan}Welcome to the Banking App{ConsoleColors.Reset}");
        var menuOptions = new string[] { $"{ConsoleColors.Green}1. Login{ConsoleColors.Reset}", $"{ConsoleColors.Yellow}2. About{ConsoleColors.Reset}", $"{ConsoleColors.Red}3. Exit{ConsoleColors.Reset}" };
        Array.ForEach(menuOptions, Console.WriteLine);

        bool Choices = false;
        while (!Choices)
        {
            string loginChoice = InputUtils.GetNonEmptyString($"{ConsoleColors.Magenta}[Type Option Number]{ConsoleColors.Reset}"); // Get user's input

            // Perform the selected action
            switch (loginChoice)
            {
                case "1":
                    Console.Clear();
                    var username = InputUtils.GetNonEmptyString($"{ConsoleColors.Yellow}Username{ConsoleColors.Reset}");
                    var password = GetHiddenPassword($"{ConsoleColors.Yellow}Password{ConsoleColors.Reset}:");
                    var captcha = GenerateRandomCaptchaValue();

                    // Display the CAPTCHA to the user
                    Console.WriteLine($"{ConsoleColors.Yellow}CAPTCHA: {captcha}{ConsoleColors.Reset}");

                    // Get user input for CAPTCHA
                    var userCaptcha = InputUtils.GetNonEmptyString($"{ConsoleColors.Magenta}Enter CAPTCHA{ConsoleColors.Reset}:");

                    // Check if CAPTCHA is correct
                    if (userCaptcha != captcha)
                    {
                        Console.WriteLine($"{ConsoleColors.Red}Invalid CAPTCHA. Please try again.{ConsoleColors.Reset}");
                        continue; // Restart the loop
                    }

                    return (username, password, userCaptcha);
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

        return (null, null, null);
    }

    /// <summary>
    /// Generates a random CAPTCHA value.
    /// </summary>
    /// <returns>The generated CAPTCHA value.</returns>
    internal string GenerateRandomCaptchaValue()
    {
        Random random = new Random();

        const string chars = "0123456789AbCdEfG";

        return new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Encrypts the password using XOR encryption.
    /// </summary>
    /// <param name="password">The password to encrypt.</param>
    /// <returns>The encrypted password.</returns>
    private string EncryptPassword(string password)
    {
        char[] passwordChars = password.ToCharArray();
        for (int i = 0; i < passwordChars.Length; i++)
        {
            passwordChars[i] = (char)(passwordChars[i] ^ 'X');
        }
        return new string(passwordChars);
    }

    /// <summary>
    /// Displays the header of the view.
    /// </summary>
    protected override void DisplayHeader()
    {
        // Header logic
    }

    /// <summary>
    /// Displays information about the developers.
    /// </summary>
    private void DisplayAbout()
    {
        var aboutNames = new string[] { "Adrian Moreno", "Alexander Doja", "Erik Berglund", "Theodor Hägg", "Yarub Adnan" };

        DisplayAboutNames(aboutNames);
        Console.ResetColor();
    }

    /// <summary>
    /// Displays the names of the developers.
    /// </summary>
    /// <param name="names">The names of the developers.</param>
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

    /// <summary>
    /// Gets the hidden password from the user.
    /// </summary>
    /// <param name="prompt">The prompt to display.</param>
    /// <returns>The hidden password.</returns>
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
