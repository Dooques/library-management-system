using library_management.Model;
using library_management.Services.Database;

namespace library_management.Services.Library;

public interface ILibraryRepository
{
    Book AddBook(Book book);
    Book DeleteBook(Book book);
    List<Book> GetBooks();
    Book? FetchBook(string title);
    Book? BorrowBook(string title);
    Book? ReturnBook(string title);
}

public class LibraryRepository(LibraryContext books) : ILibraryRepository
{
    public List<Book> GetBooks() {
        return books.Books.ToList();
    }

    public Book? FetchBook(string title)
    {
        return FindBookInDb(title);
    }

    public Book AddBook(Book book)
    {
        books.Books.Add(book);
        books.SaveChanges();
        return book;
    }

    public Book DeleteBook(Book book)
    {
        books.Books.Remove(book);
        books.SaveChanges();
        return book;
    }
    
    public Book? BorrowBook(string title)
    {
        var foundBook = FetchBook(title); 
        
        foundBook?.BorrowBook();
        return foundBook;
    }

    public Book? ReturnBook(string title)
    {
        var foundBook = FetchBook(title);
        
        foundBook?.ReturnBook();
        return foundBook;
    }
}