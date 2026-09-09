using System.Text.Json;
using library_management.Model;
using library_management.Services.Database;
using library_management.Services.IO;
using library_management.Services.Library;

using var dbContext = new LibraryContext();
// dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

var libraryRepo = new LibraryRepository(dbContext);
var libraryService = new LibraryService(libraryRepo);
var outputWriter = new OutputWriter(
    new WelcomeMessages(), 
    new ViewMessages(),
    new SearchMessages(), 
    new AddMessages(), 
    new DeleteMessages(), 
    new BorrowMessages(),
    new ReturnMessages(),
    new ErrorResponseMessages()
);

var library = new Library(libraryService, outputWriter);

if (libraryRepo.FetchBooks().Count <= 0)
{
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
}

library.Run();
