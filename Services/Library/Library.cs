using library_management.Model;
using library_management.Services.IO;

namespace library_management.Services.Library;

public class Library(
    ILibraryService libraryService, 
    IOutputWriter outputWriter
    )
{
    public bool Running = false;
    public void Run()
    {
        Running = true;
        while (Running)
        { 
            var userInput = Welcome();
            
            try
            {
                if (userInput is "exit" or "quit")
                {
                    Running = false;
                }
                else
                {
                    if (userInput.Contains("add", StringComparison.CurrentCultureIgnoreCase))
                    {
                        AddBook();
                    }
                    else if (userInput.Contains("view", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewAvailableBooks();
                    }
                    else if (userInput.Contains("search", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SearchBooks();
                    }
                    else if (userInput.Contains("delete", StringComparison.CurrentCultureIgnoreCase))
                    {
                        DeleteBook();
                    }
                    else if (userInput.Contains("borrow", StringComparison.CurrentCultureIgnoreCase))
                    {
                        BorrowBook();
                    }
                    else if (userInput.Contains("return", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ReturnBook();
                    }
                    else
                    {
                        throw new Exception("Command not recognised");
                    }
                }
            } 
            catch (Exception e) 
            { 
                outputWriter.ErrorResponse.HandleError(e); 
            }
        }
    }

    private string Welcome()
    {
        outputWriter.Welcome.WelcomeMessage();
        return outputWriter.Welcome.MenuOptions();
    }

    private void SearchBooks()
    {
        outputWriter.Welcome.WelcomeMessage();
        var type = outputWriter.Search.TypePrompt();
        if (CheckForExit(type)) return;
        
        if (!type.Contains("title", StringComparison.OrdinalIgnoreCase)
            && !type.Contains("author", StringComparison.OrdinalIgnoreCase))
            throw new Exception("Unknown search type: " + type);
        
        var searchTerm = outputWriter.Search.SearchTerm();
        if (CheckForExit(searchTerm)) return;
        
        var foundBooks = libraryService.SearchBooks(type, searchTerm);
        
        if (foundBooks.Count != 0)
        {
            outputWriter.Search.SearchResults(foundBooks);
            
            var sortBy = outputWriter.SortBy();
            if (string.IsNullOrEmpty(sortBy) || CheckForExit(sortBy)) return;
            var sortedBooks = libraryService.SortBy(sortBy, foundBooks);
            
            outputWriter.Search.SearchResults(sortedBooks);
        } 
        else outputWriter.Search.ResultsEmpty();
        
        outputWriter.ReturnToMenu();
    }

    private void ViewAvailableBooks()
    {
        outputWriter.Welcome.WelcomeMessage();
        var availableBooks = libraryService.FetchAvailableBooks();
        outputWriter.View.List(availableBooks);
        
        var sortBy = outputWriter.SortBy();
        if (string.IsNullOrEmpty(sortBy) || CheckForExit(sortBy)) return;
        var sortedBooks = libraryService.SortBy(sortBy, availableBooks);
        outputWriter.View.List(sortedBooks);

        outputWriter.ReturnToMenu();
    }

    private void AddBook()
    {
        outputWriter.Add.Begin();
        var adding = true;
        while (adding)
        {
            var title = outputWriter.Add.TitlePrompt();
            if (CheckForExit(title)) return;
            var author = outputWriter.Add.AuthorPrompt();
            if (CheckForExit(author)) return;

            try
            {
                var book = libraryService.FetchBook(title);
                if (book.Title.Contains(title) && book.Author.Contains(author))
                {
                    var bookFoundTryAgain = outputWriter.Add.BookAlreadyInLibrary(book.Title, book.Author);
                    if (bookFoundTryAgain.Contains("yes", StringComparison.OrdinalIgnoreCase)) continue;
                    return;
                }
            }
            catch (Exception)
            {
                outputWriter.Add.NoDuplicateFound();
            }

            var confirmation = outputWriter.Add.ConfirmationPrompt(title, author);
            if (CheckForExit(title)) return;
            
            if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase))
            {
                outputWriter.Add.AddBookConfirmed(title, author);
                libraryService.AddBook(title, author);
                adding = false;
            }
            else
            {
                outputWriter.Add.AddBookContinuePrompt();
                if (CheckForExit(title)) return;
            }
        }
        outputWriter.ReturnToMenu();
    }

    private void DeleteBook()
    {
        outputWriter.Welcome.WelcomeMessage();
        var deleting = true;
        while (deleting)
        {
            var title = outputWriter.Delete.TitlePrompt();
            if (CheckForExit(title)) return;
            Book book;

            try
            {
                book = libraryService.FetchBook(title);
            }
            catch(Exception e)
            {
                outputWriter.ErrorResponse.BookNotFound(title);
                return;
            }
            
            var confirmed = outputWriter.Delete.TitleConfirmationPrompt(book.Title, book.Author);
            if (CheckForExit(title)) return;
            
            if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
            {
                outputWriter.Delete.BookConfirmed(book.Title, book.Author);
                libraryService.DeleteBook(book);
                Console.ReadLine();
                deleting = false;
            }
            else
            {
                outputWriter.NotConfirmed();
            }
        }
    }

    private void BorrowBook()
    {
        outputWriter.Welcome.WelcomeMessage();
        var title = outputWriter.Borrow.TitlePrompt();
        if (CheckForExit(title)) return;
        
        Book book;
        try
        {
            book = libraryService.FetchBook(title);
        }
        catch(Exception e)
        {
            outputWriter.ErrorResponse.BookNotFound(title);
            return;
        }
        
        var confirmed = outputWriter.Borrow.ConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;

        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            outputWriter.Borrow.Confirmed(title, book.Author);
            libraryService.BorrowBook(book);
        }
        else 
            outputWriter.NotConfirmed();
        
        outputWriter.ReturnToMenu();
    }

    private void ReturnBook()
    {
        outputWriter.Welcome.WelcomeMessage();
        var title = outputWriter.Return.BookPrompt();
        if (CheckForExit(title)) return;

        Book book;
        try
        {
            book = libraryService.FetchBook(title);
        }
        catch (Exception e)
        {
            outputWriter.ErrorResponse.BookNotFound(title);
            return;
        }

        var confirmed = outputWriter.Return.ConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;
        
        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            outputWriter.Return.Confirmed(book.Title, book.Author);
            libraryService.ReturnBook(book);
        }
        else
            outputWriter.NotConfirmed();
        
        outputWriter.ReturnToMenu();
    }
    
    private static bool CheckForExit(string text)
    {
        return 
            text.Contains("exit", StringComparison.OrdinalIgnoreCase) 
            || 
            text.Contains("quit", StringComparison.OrdinalIgnoreCase);
    }
}