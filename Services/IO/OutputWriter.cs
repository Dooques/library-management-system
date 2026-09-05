namespace library_management.Services.Output;

public static class ConsoleReader
{
    public static void WelcomeMessage()
    {
        Console.WriteLine(
                """
                    Welcome to the Library Management System
                        Created by Dooques
                    With our application you can add, delete, borrow and return books from the library.
                """
            );
    }
}