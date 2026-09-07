using System.Text.Json;
using library_management.Model;
using library_management.Services.Database;
using library_management.Services.IO;
using library_management.Services.Library;

using var dbContext = new LibraryContext();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

try
{
    var seedBooks = 
        JsonSerializer.Deserialize<List<Book>>(
            File.ReadAllText("Resources/Books.json")
            ) 
        ?? throw new Exception("No books found in Resources");
    dbContext.Books.AddRange(seedBooks);
    dbContext.SaveChanges();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

var libraryRepo = new LibraryRepository(dbContext);
var libraryService = new LibraryService(libraryRepo);
var library = new Library(libraryService);

    var running = true;
    while (running)
    { 
        library.Welcome();
    
        Console.Write("    ");
        var userInput = InputReader.Read();
    
        try
        {
        if  (userInput == "exit")
        {
            running = false;
        }
        if (userInput.Contains("add", StringComparison.CurrentCultureIgnoreCase))
        {
            library.AddBook();
        }
        
        if (userInput.Contains("view", StringComparison.CurrentCultureIgnoreCase))
        {
            library.ViewBooks();
        }
        
        if (userInput.Contains("search", StringComparison.CurrentCultureIgnoreCase))
        {
            library.SearchBooks();
        }
        
        if (userInput.Contains("delete", StringComparison.CurrentCultureIgnoreCase))
        {
            library.DeleteBook();
        }
        
        if (userInput.Contains("borrow", StringComparison.CurrentCultureIgnoreCase))
        {
            library.BorrowBook();
        }
        
        if (userInput.Contains("return", StringComparison.CurrentCultureIgnoreCase))
        {
            library.ReturnBook();
        } 
    }
    catch (Exception e)
    {
        OutputWriter.ErrorResponses.HandleError(e);
    }
}