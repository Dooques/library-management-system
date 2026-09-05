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
    Console.WriteLine(seedBooks.Count);
    dbContext.Books.AddRange(seedBooks);
    dbContext.SaveChanges();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

Console.ReadLine();

var libraryRepo = new LibraryRepository(dbContext);
var libraryService = new LibraryService(libraryRepo);

var running = true;
while (running)
{ 
    OutputWriter.WelcomeMessage();
    
    Console.Write("    ");
    var userInput = InputReader.ReadInput();
    
    if  (userInput == "exit")
    {
        running = false;
    }

    if (userInput.Contains("add", StringComparison.CurrentCultureIgnoreCase))
    {
        libraryService.AddBook();
    }
    
}