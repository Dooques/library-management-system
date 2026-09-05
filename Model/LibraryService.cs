using library_management.Services.IO;

namespace library_management.Model;

public class LibraryService
{
    private LibraryRepository LibraryRepo { get; set; }

    public LibraryService(LibraryRepository libraryRepo)
    {
        LibraryRepo = libraryRepo;
    }
    public void AddBook()
    {
        OutputWriter.AddBookBegin();
        var enteringData = true;
        while (enteringData)
        {
            var title = OutputWriter.AddBookTitlePrompt();
            var author = OutputWriter.AddBookAuthorPrompt();

            var confirmation = OutputWriter.AddBookConfirmationPrompt(title, author);
            
            if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase))
            {
                OutputWriter.AddBookConfirmed(title, author);
                LibraryRepo.AddBook(new Book(title, author));
            }
            else
            {
                OutputWriter.AddBookContinuePrompt();
                if (string.Equals(confirmation, "yes", StringComparison.OrdinalIgnoreCase)) continue;
            }
            enteringData = false;
        }
        OutputWriter.AddBookReturn();
    }
}