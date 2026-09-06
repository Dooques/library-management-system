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
    public void Test1()
    {
        Assert.Pass();
    }
}