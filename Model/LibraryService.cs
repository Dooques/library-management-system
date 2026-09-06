using library_management.Services.IO;

namespace library_management.Model;

public class LibraryService(LibraryRepository libraryRepo)
{
    private LibraryRepository LibraryRepo { get; set; } = libraryRepo;

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
    }
    
    public void AddBook()
    {
        OutputWriter.Add.AddBookBegin();
        var enteringData = true;
        while (enteringData)
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
            }
            else
            {
                OutputWriter.Add.AddBookContinuePrompt();
                if (CheckForExit(title)) return;
                if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase)) continue;
            }
            enteringData = false;
        }
        OutputWriter.Add.AddBookReturn();
    }

    public static class DeleteBook
    {
        
    }
    
    public static class BorrowBook { }
    
    public static class ReturnBook {}
    
    private static bool CheckForExit(string text)
    {
        return text.Contains("exit", StringComparison.CurrentCultureIgnoreCase);
    }
}