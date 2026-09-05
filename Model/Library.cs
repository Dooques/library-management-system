using library_management.Services.Database;

namespace library_management.Model;

public interface ILibrary
{
    void AddBook(Book book);
    List<Book> GetBooks();
    Book SearchBook(string title);
    Book BorrowBook(string title);
    Book ReturnBook(string title);
}

public class Library: ILibrary
{
    private readonly LibraryContext _books;

    public Library(LibraryContext books)
    {
        _books = books;
    }
    
    public void AddBook(Book book)
    {
        _books.Books.Add(book);
        _books.SaveChanges();
    }
    
    public List<Book> GetBooks() {
        return _books.Books.ToList();
    }

    public Book SearchBook(string title)
    {
        return FindBookInDb(title);
    }

    public Book BorrowBook(string title)
    {
        var foundBook = FindBookInDb(title); 
        
        foundBook.BorrowBook();
        return foundBook;
    }

    public Book ReturnBook(string title)
    {
        var foundBook = FindBookInDb(title);
        
        foundBook.ReturnBook();
        return foundBook;
    }

    private Book FindBookInDb(string title)
    {
        return _books.Books.FirstOrDefault(b => b.Title == title) ?? 
               throw new Exception("Book not found");
    }
}