namespace library_management.Model;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
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
        IsBorrowed = true;
    }
    
    public void ReturnBook()
    {
        IsBorrowed = false;
    }

    public override string ToString()
    {
        return $"{Title} by {Author} ({IsBorrowed})";
    }
}