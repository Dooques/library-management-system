using System.Text.Json;
using library_management.Model;
using library_management.Services.Database;
using library_management.Services.IO;

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

var running = true;
while (running)
{ 
    LibraryService.WelcomeMessage();
    
    Console.Write("    ");
    var userInput = InputReader.Read();
    
    if  (userInput == "exit")
    {
        running = false;
    }

    if (userInput.Contains("add", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.AddBook();
    }
    
    if (userInput.Contains("view", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.ViewBooks();
    }
    
    if (userInput.Contains("search", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.SearchBooks();
    }
    
    if (userInput.Contains("delete", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.SearchBooks();
    }
    
    if (userInput.Contains("borrow", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.SearchBooks();
    }
    
    if (userInput.Contains("return", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.SearchBooks();
    }
}