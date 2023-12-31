﻿/// <summary>
/// Represents a console menu that allows users to navigate through options and perform actions.
/// </summary>
public class ConsoleMenu
{
    private Stack<Action> _menuStack;
    private Dictionary<string, Action> _menuOptions;
    private bool _isConsoleRedLineActive;
    private bool _isProgramRunning;

    /// <summary>
    /// Initializes a new instance of the ConsoleMenu class.
    /// </summary>
    public ConsoleMenu()
    {
        _menuStack = new Stack<Action>();
        _menuOptions = new Dictionary<string, Action>();
        _isConsoleRedLineActive = false;
        _isProgramRunning = true; // Set it to true initially
    }

    /// <summary>
    /// Adds an option to the console menu.
    /// </summary>
    /// <param name="option">The option to add.</param>
    /// <param name="action">The action to perform when the option is selected.</param>
    public void AddOption(string option, Action action)
    {
        _menuOptions.Add(option, action);
    }

    /// <summary>
    /// Checks if the program is running.
    /// </summary>
    /// <returns>True if the program is running; otherwise, false.</returns>
    public bool IsProgramRunning()
    {
        return _isProgramRunning;
    }

    /// <summary>
    /// Stops the program.
    /// </summary>
    public void StopProgram()
    {
        _isProgramRunning = false;
    }

    /// <summary>
    /// Shows the menu options on the console.
    /// </summary>
    public void ShowMenu()
    {
        foreach (var option in _menuOptions.Keys)
        {
            Console.WriteLine(option);
        }
    }

    /// <summary>
    /// Handles the user input and performs the corresponding action.
    /// </summary>
    /// <param name="input">The user input.</param>
    public void HandleInput(string input)
    {
        if (_isConsoleRedLineActive)
        {
            if (input.ToLower() == "b")
            {
                _isConsoleRedLineActive = false;
                Console.Clear();
                Console.WriteLine("Returning from Console Red Line...");
            }
            else
            {
                // Handle other commands in the Console Red Line
            }
        }
        else if (input.ToLower() == "b")
        {
            if (_menuStack.Count > 0)
            {
                var previousAction = _menuStack.Pop();
                Console.Clear();
                previousAction.Invoke();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Cannot go back any further.");
            }
        }
        else if (_menuOptions.ContainsKey(input))
        {
            if (input.ToLower() == "exit")
            {
                _isProgramRunning = false; // Set to false if the exit option is chosen
            }
            else
            {
                Console.Clear();
                _menuStack.Push(_menuOptions[input]);
                _menuOptions[input].Invoke();
            }
        }
        else
        {
            // Handle the case when input is neither "b" nor a valid menu option
            Console.WriteLine("Invalid choice. Please select a valid option.");
        }
    }

    /// <summary>
    /// Starts the console red line, allowing users to enter commands.
    /// </summary>
    public void StartConsoleRedLine()
    {
        _isConsoleRedLineActive = true;
        Console.WriteLine("Console Red Line activated. Enter commands or 'b' to return.");
        // Start a separate thread to listen for user input in the Console Red Line
        Thread consoleRedLineThread = new Thread(ConsoleRedLineThread);
        consoleRedLineThread.Start();

        // Main program loop
        while (_isProgramRunning)
        {
            ShowMenu();
            string input = Console.ReadLine();
            HandleInput(input);
            if (!_isConsoleRedLineActive)
            {
                _isProgramRunning = false; // 
            }
        }
    }

    private void ConsoleRedLineThread()
    {
        while (_isConsoleRedLineActive)
        {
            string input = Console.ReadLine();
            HandleInput(input);
        }
    }
}
