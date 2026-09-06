using System.Reflection;
using library_management.Model;

namespace library_management.Services.IO;

public static class OutputWriter
{
    public static class Welcome
    {
        public static void WelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine(
                """
                Welcome to the Library Management System
                    Created by Dooques
                    
                Type 'exit' or 'quit' to close the application at any time.
                """
            );
        }

        public static void MenuOptions()
        {
            Console.WriteLine(
                """

                With our application you can: 
                    Search
                    View
                    Add
                    Delete
                    Borrow
                    Return
                Write your command below:
                """
            );
        }
    }

    public static class View
    {
        public static void ViewBooks(List<Book> books)
        {
            Console.WriteLine(
                """
                
                Here are the currently available books:
                """
                );
            foreach (var book in books)
            {
                Console.WriteLine("    " + book);
            }
        }
    }

    public static class Search
    {
        public static string SearchByPrompt()
        {
            Console.WriteLine(
                """
                
                Would you like to search by title or author?
                """
            );
            return InputReader.Read();
        }

        public static string SearchTerm()
        {
            Console.WriteLine(
                """
                
                Enter your search:
                """
            );
            return InputReader.Read();
        }

        public static void SearchResults(List<Book> books)
        {
            Console.WriteLine(
                """
                
                Here is what we found:
                """
                );
            foreach (var book in books)
            {
                Console.WriteLine("    " + book);
            }
        }

        public static void SearchResultsEmpty()
        {
            Console.WriteLine(
                """
                
                No results found.
                """
            );
        } 
    }
    
    public static class Add 
    { 
        public static void AddBookBegin() 
        { 
            Console.WriteLine(
            """

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
            return InputReader.Read();
        }

        public static string AddBookAuthorPrompt()
        {
            Console.WriteLine(
                """

                Thank you, now enter the author of this book:
                """
            );

            Console.Write("    ");
            return InputReader.Read();
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
            return InputReader.Read();
        }

        public static void AddBookConfirmed(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""

                 {bookTitle} by {bookAuthor} has been added to the library.
                 """
            );
        }

        public static string BookAlreadyInLibrary(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""
                
                The book {bookTitle} by {bookAuthor} is already in the library, would you like to add something else?
                Type Yes or No:
                """
                );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static string AddBookContinuePrompt()
        {
            Console.WriteLine(
                """

                Would you like to retry entering the book information?
                """
            );

            Console.Write("    ");
            return InputReader.Read();
        }

    }

    public static class Delete
    {
        public static string DeleteTitlePrompt()
        {
            Console.WriteLine(
                """
                
                Enter the title of the book to delete:
                """
                );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static string DeleteTitleConfirmationPrompt(string title, string author)
        {
            Console.WriteLine(
                $"""
                
                Are you sure you want to delete {title} by {author}?
                """
                );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static void DeleteBookConfirmed(string title, string author)
        {
            Console.WriteLine(
                $"""
                
                {title}  by {author} has been deleted.
                """
            );
        }

    }

    public static class Borrow
    {
        public static string BorrowBookPrompt()
        {
            Console.WriteLine(
                """
                
                What is the title of the book are you borrowing?
                """
            );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static string BorrowBookConfirmationPrompt(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""
                
                Are you trying to borrow {bookTitle} by {bookAuthor}?
                """
                );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static void BorrowBookConfirmed(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""
                
                {bookTitle} by {bookAuthor} has been borrowed.
                """
                );
        }
    }

    public static class Return
    {
        public static string ReturnBookPrompt()
        {
            Console.WriteLine(
                """

                What is the title of the book you are returning?
                """
            );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static string ReturnBookConfirmationPrompt(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""

                 Are you trying to return {bookTitle} by {bookAuthor}?
                 """
            );
            
            Console.Write("    ");
            return InputReader.Read();
        }

        public static void ReturnBookConfirmed(string bookTitle, string bookAuthor)
        {
            Console.WriteLine(
                $"""

                 {bookTitle} by {bookAuthor} has been borrowed.
                 """
            );
        }
        
        
    }

    public static void NotConfirmed()
    {
        Console.WriteLine(
            """

            Let's start over...
            """
        );
    }
    
    public static void ReturnToMenu()
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