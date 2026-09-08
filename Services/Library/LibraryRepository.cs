using library_management.Model;
using library_management.Services.Database;

namespace library_management.Services.Library;

public interface ILibraryRepository
{
    Book AddBook(Book book);
    Book DeleteBook(Book book);
    List<Book> FetchBooks();
    Book FetchBook(string title);
    Book BorrowBook(Book book);
    Book ReturnBook(Book book);
}

public class LibraryRepository(LibraryContext books) : ILibraryRepository
{
    public List<Book> FetchBooks() {
        return books.Books.ToList();
    }

    public Book FetchBook(string title)
    {
        if (string.IsNullOrEmpty(title)) throw new Exception("Search should not be empty");
        
        return books.Books.FirstOrDefault(b => b.Title == title) ?? 
               throw new Exception("Book not found");
    }

    public Book AddBook(Book book)
    {
        if (books.Books.Any(b => b.Title == book.Title && b.Author == book.Author)) throw new Exception("Book already exists");
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