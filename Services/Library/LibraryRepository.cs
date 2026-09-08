using library_management.Model;
using library_management.Services.Database;

namespace library_management.Services.Library;

public interface ILibraryRepository
{
    Book AddBook(Book book);
    Book DeleteBook(Book book);
    List<Book> GetBooks();
    Book FetchBook(string title);
    Book BorrowBook(Book book);
    Book ReturnBook(Book book);
}

public class LibraryRepository(LibraryContext books) : ILibraryRepository
{
    public List<Book> GetBooks() {
        return books.Books.ToList();
    }

    public Book FetchBook(string title)
    {
        return string.IsNullOrEmpty(title) ? 
            throw new Exception("Search should not be empty") : 
            books.Books.FirstOrDefault(b => b.Title == title);
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
    
    public Book BorrowBook(Book book)
    {
        book.BorrowBook();
        return book;
    }

    public Book ReturnBook(Book book)
    {
        book.ReturnBook();
        return book;
    }
}