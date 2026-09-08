using System.ComponentModel.DataAnnotations;

namespace library_management.Model;

public class Book
{
    public int Id { get; init; }
    
    [StringLength(100)]
    public string Title { get; init; }
    
    [StringLength(100)]
    public string Author { get; init; }
    
    public bool IsBorrowed { get; set; }

    public Book(string title, string author)
    {
        Id = 0;
        Title = title;
        Author = author;
        IsBorrowed = false;
    }

    public void BorrowBook()
    {
        if (IsBorrowed) throw new Exception("Book already borrowed");
        IsBorrowed = true;
    }
    
    public void ReturnBook()
    {
        if (!IsBorrowed) throw new Exception("Book is still available");
        IsBorrowed = false;
    }

    public override string ToString()
    {
        var available = IsBorrowed ? "Unavailable": "Available";
        return $"{Title} by {Author} ({available})";
    }
}