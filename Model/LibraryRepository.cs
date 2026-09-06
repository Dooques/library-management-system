using library_management.Services.Database;

namespace library_management.Model;

public interface ILibraryRepository
{
    void AddBook(Book book);
    List<Book> GetBooks();
    Book? SearchBook(string title);
    Book? BorrowBook(string title);
    Book? ReturnBook(string title);
}

public class LibraryRepository(LibraryContext books) : ILibraryRepository
{
    public void AddBook(Book book)
    {
        books.Books.Add(book);
        books.SaveChanges();
    }
    
    public List<Book> GetBooks() {
        return books.Books.ToList();
    }

    public Book? SearchBook(string title)
    {
        return FindBookInDb(title);
    }

    public Book? BorrowBook(string title)
    {
        var foundBook = FindBookInDb(title); 
        
        foundBook?.BorrowBook();
        return foundBook;
    }

    public Book? ReturnBook(string title)
    {
        var foundBook = FindBookInDb(title);
        
        foundBook?.ReturnBook();
        return foundBook;
    }

    private Book? FindBookInDb(string title)
    {
        return books.Books.FirstOrDefault(b => b.Title == title);
    }
}