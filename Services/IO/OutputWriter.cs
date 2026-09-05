using library_management.Model;

namespace library_management.Services.IO;

public static class OutputWriter
{
    public static void WelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine(
            """
            Welcome to the Library Management System
                Created by Dooques
        
            With our application you can: 
                Search
                Add
                Delete
                Borrow
                Return
            Write your command below:
            """
            );
    }

    public static void AddBookBegin()
    {
        Console.Clear();

        Console.WriteLine(
            """
            Welcome to the Library Management System
                Created by Dooques

            You have selected "Add Book".
            """
        );
    }

    public static string AddBookTitlePrompt()
    {
        Console.WriteLine(
            """

            Please enter the title of the book you would like to add.
            """
        );

        Console.Write("    ");
        return InputReader.ReadInput();
    }

    public static string AddBookAuthorPrompt()
    {
        Console.WriteLine(
            """

            Thank you, now enter the author of this book:
            """
        );

        Console.Write("    ");
        return InputReader.ReadInput();
    }

    public static string AddBookConfirmationPrompt(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""

             You have given the following information:
                Title: {bookTitle}
                Author: {bookAuthor}
                     
             Is this correct?
             Type Yes or No:
             """
        );

        Console.Write("    ");
        return InputReader.ReadInput();
    }

    public static void AddBookConfirmed(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""

             {bookTitle} by {bookAuthor} has been added to the library.
             """
        );
    }

    public static string AddBookContinuePrompt()
    {
        Console.WriteLine(
            """

            Would you like to retry entering the book information?
            """
        );

        Console.Write("    ");
        return InputReader.ReadInput();
    }

    public static void AddBookReturn()
    {
        Console.WriteLine(
            """

            Returning to the main menu...
            
            Press Enter to continue
            """
        );
        Console.ReadLine();
    }
}