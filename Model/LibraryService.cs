using library_management.Services.IO;

namespace library_management.Model;

public class LibraryService(ILibraryRepository libraryRepo)
{
    private ILibraryRepository LibraryRepo { get; set; } = libraryRepo;

    public static void WelcomeMessage()
    {
        OutputWriter.Welcome.WelcomeMessage();
        OutputWriter.Welcome.MenuOptions();
    }

    public void ViewBooks()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var books = LibraryRepo.GetBooks();
        OutputWriter.View.ViewBooks(books);
        OutputWriter.ReturnToMenu();
    }

    public void SearchBooks()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var type = OutputWriter.Search.SearchByPrompt();
        if (CheckForExit(type)) return;
        var searchTerm =  OutputWriter.Search.SearchTerm(type);
        if (CheckForExit(searchTerm)) return;
        var books = LibraryRepo.GetBooks();
        var searchResult = type switch
        {
            "title" => books.FindAll(b => b.Title.Contains(searchTerm)),
            "author" => books.FindAll(b => b.Author.Contains(searchTerm)),
            _ => throw new Exception("Unknown search type: " + type)
        };
        
        if (searchResult.Any())
        {
            OutputWriter.Search.SearchResults(searchResult);
        } 
        else OutputWriter.Search.SearchResultsEmpty();
        Console.ReadLine();
        
        OutputWriter.ReturnToMenu();
    }
    
    public void AddBook()
    {
        OutputWriter.Add.AddBookBegin();
        var adding = true;
        while (adding)
        {
            var title = OutputWriter.Add.AddBookTitlePrompt();
            if (CheckForExit(title)) return;
            var author = OutputWriter.Add.AddBookAuthorPrompt();
            if (CheckForExit(title)) return;

            var confirmation = OutputWriter.Add.AddBookConfirmationPrompt(title, author);
            if (CheckForExit(title)) return;
            
            if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase))
            {
                OutputWriter.Add.AddBookConfirmed(title, author);
                LibraryRepo.AddBook(new Book(title, author));
                adding = false;
            }
            else
            {
                OutputWriter.Add.AddBookContinuePrompt();
                if (CheckForExit(title)) return;
                if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase)) continue;
            }
        }
        OutputWriter.ReturnToMenu();
    }

    public void DeleteBook()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var deleting = true;
        while (deleting)
        {
            var title = OutputWriter.Delete.DeleteTitlePrompt();
            if (CheckForExit(title)) return;
            
            var book = LibraryRepo.SearchBook(title);
            var confirmed = OutputWriter.Delete.DeleteTitleConfirmationPrompt(book.Title, book.Author);
            if (CheckForExit(title)) return;
            
            if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
            {
                OutputWriter.Delete.DeleteBookConfirmed(book.Title, book.Author);
                Console.ReadLine();
                deleting = false;
            }
            else
            {
                OutputWriter.NotConfirmed();
            }
        }
    }

    public void BorrowBook()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var title = OutputWriter.Borrow.BorrowBookPrompt();
        if (CheckForExit(title)) return;
        
        var book = LibraryRepo.SearchBook(title);
        var confirmed = OutputWriter.Borrow.BorrowBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;
        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase)) 
            OutputWriter.Borrow.BorrowBookConfirmed(title, book.Author);
        else 
            OutputWriter.NotConfirmed();
        
        OutputWriter.ReturnToMenu();
    }

    public void ReturnBook()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var title = OutputWriter.Return.ReturnBookPrompt();
        if (CheckForExit(title)) return;
        
        var book = LibraryRepo.SearchBook(title);
        
        var confirmed = OutputWriter.Return.ReturnBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;
        
        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
            OutputWriter.Return.ReturnBookConfirmed(title, book.Author);
        else
            OutputWriter.NotConfirmed();
        
        OutputWriter.ReturnToMenu();
    }
    
    private static bool CheckForExit(string text)
    {
        return text.Contains("exit", StringComparison.CurrentCultureIgnoreCase);
    }
}