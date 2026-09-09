using System.Reflection;
using library_management.Model;

namespace library_management.Services.IO;

public interface IOutputWriter
{
    public WelcomeMessages Welcome { get; }
    public ViewMessages View { get; }
    public SearchMessages Search { get; }
    public AddMessages Add { get; }
    public DeleteMessages Delete { get; }
    public BorrowMessages Borrow { get; }
    public ReturnMessages Return { get; }
    public ErrorResponseMessages ErrorResponse { get; }

    public void NotConfirmed();
    public void  ReturnToMenu();
}

public class OutputWriter(
    WelcomeMessages welcome,
    ViewMessages view,
    SearchMessages search,
    AddMessages add,
    DeleteMessages delete,
    BorrowMessages borrow,
    ReturnMessages @return, 
    ErrorResponseMessages errorResponse
    ): IOutputWriter
{
    public WelcomeMessages Welcome { get; } = welcome;
    public ViewMessages View { get; } = view;
    public SearchMessages Search { get; } = search;
    public AddMessages Add { get; }= add;
    public DeleteMessages Delete { get; }= delete;
    public BorrowMessages Borrow { get; }= borrow;
    public ReturnMessages Return { get; }= @return;
    public ErrorResponseMessages ErrorResponse { get; }= errorResponse;

    public void NotConfirmed()
    {
        Console.WriteLine(
            """

            Let's start over...
            """
        );
        Console.ReadLine();
    }
    
    public void ReturnToMenu()
    {
        Console.WriteLine(
            """

            Press Enter to return to the menu:
            """
        );
        Console.ReadLine(); 
    } 
}

public class WelcomeMessages
    {
        private readonly  InputReader _inputReader = new();
        public void WelcomeMessage()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                
            }

            Console.WriteLine(
                """
                Welcome to the Library Management System
                    Created by Dooques
                    
                Type 'exit' or 'quit' to close the application at any time.
                """
            );
            
        }

        public string MenuOptions()
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
            
            Console.Write("    ");
            return _inputReader.Read();
        }
    }

public class ViewMessages
{
    public void List(List<Book> books)
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

public class SearchMessages()
{
    private readonly InputReader _inputReader = new();
    public  string TypePrompt()
    {
        Console.WriteLine(
            """
            
            Would you like to search by title or author?
            """
        );
        return _inputReader.Read();
    }

    public  string SearchTerm()
    {
        Console.WriteLine(
            """
            
            Enter your search:
            """
        );
        return _inputReader.Read();
    }

    public  void SearchResults(List<Book> books)
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

    public void ResultsEmpty()
    {
        Console.WriteLine(
            """
            
            No results found.
            """
        );
    } 
}

public class AddMessages()
{
    private readonly InputReader _inputReader = new InputReader();
    public  void Begin() 
    { 
        Console.WriteLine(
        """

        You have selected "Add Book".
        """
        );
    }

    public  string TitlePrompt()
    {
        Console.WriteLine(
            """

            Please enter the title of the book you would like to add.
            """
        );

        Console.Write("    ");
        return _inputReader.Read();
    }

    public string AuthorPrompt()
    {
        Console.WriteLine(
            """

            Thank you, now enter the author of this book:
            """
        );

        Console.Write("    ");
        return _inputReader.Read();
    }

    public  string ConfirmationPrompt(string bookTitle, string bookAuthor)
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
        return _inputReader.Read();
    }

    public void NoDuplicateFound()
    {
        Console.WriteLine(
            """
            
            No duplicate found, ok to continue...
            """
            );
        Console.ReadLine();
    }

    public  void AddBookConfirmed(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""

             {bookTitle} by {bookAuthor} has been added to the library.
             """
        );
    }

    public  string BookAlreadyInLibrary(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""
            
            The book {bookTitle} by {bookAuthor} is already in the library, would you like to add something else?
            Type Yes or No:
            """
            );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  string AddBookContinuePrompt()
    {
        Console.WriteLine(
            """

            Would you like to retry entering the book information?
            """
        );

        Console.Write("    ");
        return _inputReader.Read();
    }

}

public  class DeleteMessages()
{
    private readonly InputReader _inputReader = new InputReader();
    public  string TitlePrompt()
    {
        Console.WriteLine(
            """
            
            Enter the title of the book to delete:
            """
            );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  string TitleConfirmationPrompt(string title, string author)
    {
        Console.WriteLine(
            $"""
            
            Are you sure you want to delete {title} by {author}?
            """
            );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  void BookConfirmed(string title, string author)
    {
        Console.WriteLine(
            $"""
            
            {title}  by {author} has been deleted.
            """
        );
    }

}

public  class BorrowMessages()
{
    private readonly InputReader _inputReader = new InputReader();
    public  string TitlePrompt()
    {
        Console.WriteLine(
            """
            
            What is the title of the book are you borrowing?
            """
        );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  string ConfirmationPrompt(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""
            
            Are you trying to borrow {bookTitle} by {bookAuthor}?
            """
            );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  void Confirmed(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""
            
            {bookTitle} by {bookAuthor} has been borrowed.
            """
            );
    }
}

public  class ReturnMessages()
{
    private readonly  InputReader _inputReader = new InputReader();
    public  string BookPrompt()
    {
        Console.WriteLine(
            """

            What is the title of the book you are returning?
            """
        );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  string ConfirmationPrompt(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""

             Are you trying to return {bookTitle} by {bookAuthor}?
             """
        );
        
        Console.Write("    ");
        return _inputReader.Read();
    }

    public  void Confirmed(string bookTitle, string bookAuthor)
    {
        Console.WriteLine(
            $"""

             {bookTitle} by {bookAuthor} has been returned.
             """
        );
    }
    
    
}

public class ErrorResponseMessages
{
    public  void HandleError(Exception e)
    {
        Console.WriteLine(
            $"""
            
            Looks like something went wrong:
                {e.Message}
                
            Press enter to start over:
            """
            );
        
        Console.ReadLine();
    }

    public void BookNotFound(string title)
    {
        Console.WriteLine(
            $"""
            
            {title} was not found, press enter to return to the menu:
            """
        );
        Console.ReadLine();
    }
    
    public  void BookFound(string title)
    {
        Console.WriteLine(
            $"""

             {title} is already in the library, press enter to return to the menu:
             """
        );
        Console.ReadLine();
    }
}
    