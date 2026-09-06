using library_management.Model;

namespace LibraryTesting;

public class BookTests
{
    private Book _testBook;
    
    [SetUp]
    public void Setup()
    {
        _testBook = new Book("test", "john test");
    }

    [Test]
    public void CreateBook_ShouldHaveTitleAndAuthor()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_testBook.Title, Is.EqualTo("test"));
            Assert.That(_testBook.Author, Is.EqualTo("john test"));
        });
    }

    [Test]
    public void BorrowBook_BorrowedFieldShouldBecomeTrue()
    {
        
        _testBook.BorrowBook();
        Assert.That(_testBook.IsBorrowed, Is.True);
    }
    
    [Test]
    public void ReturnBook_BorrowedFieldShouldBecomeFalse()
    {
        _testBook.BorrowBook();
        _testBook.ReturnBook();
        Assert.That(_testBook.IsBorrowed, Is.False);
    }
    
    [Test]
    public void ToString_ShouldReturnBookTitleAuthorAndAvailability()
    {
        Assert.That(_testBook.ToString(), 
            Is.EqualTo("test by john test (Available)"));
    }
}