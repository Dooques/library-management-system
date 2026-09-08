using library_management.Model;
using library_management.Services.IO;

namespace library_management.Services.Library;

public class Library(ILibraryService libraryService, OutputWriter outputWriter)
{
    public void Run()
    {
        var running = true;
        while (running)
        { 
            Welcome();

            Console.Write("    ");
            var userInput = InputReader.Read();

            try
            {
                if (userInput == "exit")
                {
                    running = false;
                }
                else
                {
                    if (userInput.Contains("add", StringComparison.CurrentCultureIgnoreCase))
                    {
                        AddBook();
                    }
                    else if (userInput.Contains("view", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewBooks();
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
                }
            } 
            catch (Exception e) 
            { 
                outputWriter.ErrorResponse.HandleError(e); 
            }
        }
    }

    private void Welcome()
    {
        outputWriter.Welcome.WelcomeMessage();
        outputWriter.Welcome.MenuOptions();
    }

    private void SearchBooks()
    {
        outputWriter.Welcome.WelcomeMessage();
        var type = outputWriter.Search.SearchByPrompt();
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
        } 
        else outputWriter.Search.SearchResultsEmpty();
        
        outputWriter.ReturnToMenu();
    }

    private void ViewBooks()
    {
        outputWriter.Welcome.WelcomeMessage();
        outputWriter.View.ViewBooks(libraryService.GetBooks());
        outputWriter.ReturnToMenu();
    }

    private void AddBook()
    {
        outputWriter.Add.AddBookBegin();
        var adding = true;
        while (adding)
        {
            var title = outputWriter.Add.AddBookTitlePrompt();
            if (CheckForExit(title)) return;
            var author = outputWriter.Add.AddBookAuthorPrompt();
            if (CheckForExit(title)) return;

            var book = libraryService.FetchBook(title);

            if (book != null && book.Author.Contains(author))
            {
                var bookFoundTryAgain = outputWriter.Add.BookAlreadyInLibrary(book.Title, book.Author);
                if (bookFoundTryAgain.Contains("yes", StringComparison.OrdinalIgnoreCase)) continue;
                return;
            }

            var confirmation = outputWriter.Add.AddBookConfirmationPrompt(title, author);
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
            var title = outputWriter.Delete.DeleteTitlePrompt();
            if (CheckForExit(title)) return;
            
            var book = libraryService.FetchBook(title);
            if (book == null)
            {
                outputWriter.ErrorResponse.BookNotFound(title);
                return;
            }
            
            var confirmed = outputWriter.Delete.DeleteTitleConfirmationPrompt(book.Title, book.Author);
            if (CheckForExit(title)) return;
            
            if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
            {
                outputWriter.Delete.DeleteBookConfirmed(book.Title, book.Author);
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
        var title = outputWriter.Borrow.BorrowBookPrompt();
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
        
        var confirmed = outputWriter.Borrow.BorrowBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;

        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            outputWriter.Borrow.BorrowBookConfirmed(title, book.Author);
            libraryService.BorrowBook(book);
        }
        else 
            outputWriter.NotConfirmed();
        
        outputWriter.ReturnToMenu();
    }

    private void ReturnBook()
    {
        outputWriter.Welcome.WelcomeMessage();
        var title = outputWriter.Return.ReturnBookPrompt();
        if (CheckForExit(title)) return;
        
        var book = libraryService.FetchBook(title);

        var confirmed = outputWriter.Return.ReturnBookConfirmationPrompt(book.Title, book.Author);
        if (CheckForExit(title)) return;
        
        if (confirmed.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            outputWriter.Return.ReturnBookConfirmed(book.Title, book.Author);
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