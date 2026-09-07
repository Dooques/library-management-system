using library_management.Model;

namespace library_management.Services.Library;

public interface ILibraryService
{
    public List<Book> GetBooks();
    public Book? FetchBook(string title);
    public List<Book> SearchBooks(string type, string searchTerm);
    public Book AddBook(string title, string author);
    public Book DeleteBook(Book book);
    public Book BorrowBook(Book book);
    public Book ReturnBook(Book book);
}

public class LibraryService(ILibraryRepository libraryRepository): ILibraryService
{
    public List<Book> GetBooks()
    {
        return libraryRepository.GetBooks();
    }

    public Book? FetchBook(string title)
    {
        try
        {
            return libraryRepository.FetchBook(title);
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public List<Book> SearchBooks(string type, string searchTerm)
    {
        var books = libraryRepository.GetBooks();
        
        var searchResult = type switch
        {
            "title" => books.FindAll(b => 
                b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)),
            "author" => books.FindAll(b => 
                b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)),
            _ => throw new ArgumentOutOfRangeException("Invalid search type: " + type)
        };

        return searchResult;
    }

    public Book AddBook(string title, string author)
    {
        return libraryRepository.AddBook(new Book(title, author));
    }

    public Book DeleteBook(Book book)
    {
        libraryRepository.DeleteBook(book);
        return book;
    }

    public Book BorrowBook(Book book)
    {
        if (book.IsBorrowed) throw new Exception("Book already borrowed");
        
        libraryRepository.BorrowBook(book.Title);
        return book;
    }

    public Book ReturnBook(Book book)
    {
        if (!book.IsBorrowed) throw new Exception("Book not borrowed");
        libraryRepository.ReturnBook(book.Title);
        return book;
    }
}