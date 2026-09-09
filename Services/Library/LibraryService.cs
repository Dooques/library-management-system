using library_management.Model;

namespace library_management.Services.Library;

public interface ILibraryService
{
    public List<Book> FetchAvailableBooks();
    public Book FetchBook(string title);
    public List<Book> SearchBooks(string type, string searchTerm);
    public Book AddBook(string title, string author);
    public Book DeleteBook(Book book);
    public Book BorrowBook(Book book);
    public Book ReturnBook(Book book);
    public List<Book> SortBy(string sortBy, List<Book> books);
}

public class LibraryService(ILibraryRepository libraryRepository): ILibraryService
{
    public List<Book> FetchAvailableBooks()
    {
        return libraryRepository.FetchBooks().FindAll(b => !b.IsBorrowed);
    }

    public Book FetchBook(string title)
    {
        return libraryRepository.FetchBook(title);
    }
    
    public List<Book> SearchBooks(string type, string searchTerm)
    {
        if (string.IsNullOrEmpty(type)) 
            throw new ArgumentNullException(nameof(type));
        
        if (string.IsNullOrEmpty(searchTerm)) 
            throw new ArgumentNullException(nameof(searchTerm));
        
        var books = libraryRepository.FetchBooks();
        
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
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
        if (string.IsNullOrEmpty(author)) throw new ArgumentNullException(nameof(author));

        return libraryRepository.AddBook(new Book(title, author));
    }

    public Book DeleteBook(Book book)
    {
        libraryRepository.DeleteBook(book);
        return book;
    }

    public Book BorrowBook(Book book)
    {
        var borrowedBook = libraryRepository.BorrowBook(book);
        return borrowedBook;
    }

    public Book ReturnBook(Book book)
    {
        var returnedBook = libraryRepository.ReturnBook(book);
        return returnedBook;
    }

    public List<Book> SortBy(string sortBy, List<Book> books)
    {
        ArgumentNullException.ThrowIfNull(books);

        List<Book> sortResult;
        if (sortBy.Contains("title", StringComparison.OrdinalIgnoreCase))
        {
            sortResult = (sortBy.Contains('Z', StringComparison.OrdinalIgnoreCase)
                ? books.OrderByDescending(a => a.Title)
                : books.OrderBy(a => a.Title)).ToList();
        } 
        else if (sortBy.Contains("author",  StringComparison.OrdinalIgnoreCase))
        {
            sortResult = (sortBy.Contains('Z', StringComparison.OrdinalIgnoreCase)
                ? books.OrderByDescending(a => a.Author)
                : books.OrderBy(a => a.Author)).ToList();
        }
        else
        {
            throw new ArgumentNullException(nameof(sortBy));
        }

        return sortResult;
    }
}