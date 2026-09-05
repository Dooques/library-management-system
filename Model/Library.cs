using library_management.Services.Database;

namespace library_management.Model;

public class Library
{
    private LibraryContext _books;

    public Library(LibraryContext books)
    {
        _books = books;
    }
    
    public void  AddBook(Book book)
    {
        _books.Add(book);
    }
    
    public List<Book> GetBooks() {
        return _books.Books.ToList();
    }

    public Book SearchBook(string title)
    {
        var foundBook = _books.Books.FirstOrDefault(b => b.Title == title);
        return foundBook ?? throw new Exception("Book not found");
    }

    public Book BorrowBook(string title)
    {
        var foundBook = _books.Books.FirstOrDefault(b => b.Title == title);
        
        if (foundBook == null) throw new Exception("Book not found");
        
        foundBook.BorrowBook();
        return foundBook;
    }

    public Book ReturnBook(string title)
    {
        var foundBook = _books.Books.FirstOrDefault(b => b.Title == title);
        
        if (foundBook == null) throw new Exception("Book not found");
        
        foundBook.ReturnBook();
        return foundBook;
    }
}