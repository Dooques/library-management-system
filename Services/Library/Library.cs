using library_management.Services.IO;
using library_management.Services.Library;

namespace library_management.Model;

public class Library(ILibraryService libraryService)
{
    private ILibraryService _libraryService = libraryService;

    public void Welcome()
    {
        OutputWriter.Welcome.WelcomeMessage();
        OutputWriter.Welcome.MenuOptions();
    }
    
    public void SearchBooks()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var type = OutputWriter.Search.SearchByPrompt();
        if (CheckForExit(type)) return;
        
        if (!type.Contains("title", StringComparison.OrdinalIgnoreCase)
            && !type.Contains("author", StringComparison.OrdinalIgnoreCase))
            throw new Exception("Unknown search type: " + type);
        
        var searchTerm = OutputWriter.Search.SearchTerm();
        if (CheckForExit(searchTerm)) return;
        
        var foundBooks = _libraryService.SearchBooks(type, searchTerm);
        
        if (foundBooks.Count != 0)
        {
            OutputWriter.Search.SearchResults(foundBooks);
        } 
        else OutputWriter.Search.SearchResultsEmpty();
        
        OutputWriter.ReturnToMenu();
    }

    public void ViewBooks()
    {
        OutputWriter.Welcome.WelcomeMessage();
        OutputWriter.View.ViewBooks(_libraryService.GetBooks());
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

            var book = _libraryService.SearchBook(title);

            if (book != null && book.Author.Contains(author))
            {
                var bookFoundTryAgain = OutputWriter.Add.BookAlreadyInLibrary(book.Title, book.Author);
                if (bookFoundTryAgain.Contains("yes", StringComparison.OrdinalIgnoreCase)) continue;
                return;
            }

            var confirmation = OutputWriter.Add.AddBookConfirmationPrompt(title, author);
            if (CheckForExit(title)) return;
            
            if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase))
            {
                OutputWriter.Add.AddBookConfirmed(title, author);
                _libraryService.AddBook(title, author);
                adding = false;
            }
            else
            {
                OutputWriter.Add.AddBookContinuePrompt();
                if (CheckForExit(title)) return;
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
            
            var book = _libraryService.SearchBook(title);
            if (book == null)
            {
                OutputWriter.ErrorResponses.BookNotFound(title);
                return;
            }
            
            var confirmed = OutputWriter.Delete.DeleteTitleConfirmationPrompt(book.Title, book.Author);
            if (CheckForExit(title)) return;
            
            if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
            {
                OutputWriter.Delete.DeleteBookConfirmed(book.Title, book.Author);
                _libraryService.DeleteBook(book);
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
        
        var book = _libraryService.SearchBook(title);
        if (book == null)
        {
            OutputWriter.ErrorResponses.BookNotFound(title);
            return;
        }
        
        var confirmed = OutputWriter.Borrow.BorrowBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;

        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            OutputWriter.Borrow.BorrowBookConfirmed(title, book.Author);
            _libraryService.BorrowBook(book);
        }
        else 
            OutputWriter.NotConfirmed();
        
        OutputWriter.ReturnToMenu();
    }
    
    public void ReturnBook()
    {
        OutputWriter.Welcome.WelcomeMessage();
        var title = OutputWriter.Return.ReturnBookPrompt();
        if (CheckForExit(title)) return;
        
        var book = _libraryService.SearchBook(title);
        if (book == null)
        {
            OutputWriter.ErrorResponses.BookNotFound(title);
            return;
        }
        
        var confirmed = OutputWriter.Return.ReturnBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;
        
        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            OutputWriter.Return.ReturnBookConfirmed(book.Title, book.Author);
            _libraryService.ReturnBook(book);
        }
        else
            OutputWriter.NotConfirmed();
        
        OutputWriter.ReturnToMenu();
    }
    
    private static bool CheckForExit(string text)
    {
        return 
            text.Contains("exit", StringComparison.OrdinalIgnoreCase) 
            || 
            text.Contains("quit", StringComparison.OrdinalIgnoreCase);
    }
}