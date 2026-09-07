using System.Diagnostics;
using library_management.Model;
using library_management.Services.Library;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryTesting;

public class LibraryServiceTesting
{
    private Mock<ILibraryRepository> _mockLibraryRepo;
    private LibraryService _libraryService;

    private readonly List<Book> _booklist =
    [
        new Book("Dune", "Frank Herbert"),
        new Book("Neuromancer", "William Gibson"),
        new Book("Foundation", "Isaac Asimov")
    ];
    
    [SetUp]
    public void Setup()
    {
        _mockLibraryRepo = new Mock<ILibraryRepository>();
        _libraryService = new LibraryService(_mockLibraryRepo.Object);
    }

    [Test]
    public void GetBooks_ValidBooks_ShouldReturnAllBooks()
    {
        _mockLibraryRepo.Setup(x => 
            x.GetBooks()).Returns(_booklist);
        var books = _libraryService.GetBooks();
        
        Assert.That(books, Has.Count.EqualTo(_booklist.Count));
        Assert.That(books[0], Is.EqualTo(_booklist[0]));
    }
    
    [Test]
    public void GetBooks_InvalidBooks_ShouldReturnNoBooks()
    {
        _mockLibraryRepo.Setup(x => x.GetBooks()).Returns([]);
        var books = _libraryService.GetBooks();
        
        Assert.That(books, Is.Empty);
    }

    [Test]
    public void FetchBook_ValidSearch_ShouldReturnNeuromancer()
    {
        _mockLibraryRepo.Setup(x => 
            x.FetchBook(It.IsAny<string>())).Returns(_booklist[0]);
        
        var book = _libraryService.FetchBook("Neuromancer");
        Assert.That(book, Is.EqualTo(_booklist[0]));
    }
    
    [Test]
    public void FetchBook_InvalidSearch_ShouldReturnNull()
    {
        _mockLibraryRepo.Setup(x => 
            x.FetchBook(string.Empty)).Returns((Book)null!);
        
        Assert.That(_libraryService.FetchBook(""), Is.Null);
    }

    [Test]
    public void FetchBook_ValidSearch_ReturnNoResults()
    {
        _mockLibraryRepo.Setup(x => 
            x.FetchBook(It.IsAny<string>())).Returns((Book)null!);
        
        Assert.That(_libraryService.FetchBook("Jungle Book"), Is.Null);
    }

    [Test]
    public void SearchBooks_TypeTitleValidSearch_ReturnsOne()
    {
        _mockLibraryRepo.Setup(x => x.GetBooks()).Returns(_booklist);
        
        var books = _libraryService.SearchBooks("title", "Neuromancer");
        Assert.That(books, Has.Count.EqualTo(1));
        Assert.That(books[0], Is.EqualTo(_booklist[1]));
    }
    
    [Test]
    public void SearchBooks_TypeTitleInvalidSearch_ThrowArgumentNullExcpetion()
    {
        _mockLibraryRepo.Setup(x => x.GetBooks()).Returns(_booklist);
        Assert.Throws<ArgumentNullException>(() => 
            _libraryService.SearchBooks("title", ""));
    }
    
    [Test]
    public void SearchBooks_TypeAuthorInvalidSearch_ThrowArgumentNullExcpetion()
    {
        _mockLibraryRepo.Setup(x => x.GetBooks()).Returns(_booklist);
        Assert.Throws<ArgumentNullException>(() => 
            _libraryService.SearchBooks("author", ""));
    }

    [Test]
    public void AddBook_ValidArguments_Success()
    {
        _mockLibraryRepo.Setup(x => x.AddBook(It.IsAny<Book>())).Returns(_booklist[0]);
        
        var book = _libraryService.AddBook("Dune", "Frank Herbert");
        Assert.That(book, Is.EqualTo(_booklist[0]));
    }
    
    [Test]
    public void AddBook_TitleEmpty_DoesNotAdd()
    {
        _mockLibraryRepo.Setup(x => 
            x.AddBook(It.IsAny<Book>())).Throws<ArgumentNullException>();
        
        Assert.Throws<ArgumentNullException>(() => _libraryService.AddBook("", "Frank Herbert"));
    }
    
    [Test]
    public void AddBook_AuthorEmpty_DoesNotAdd()
    {
        _mockLibraryRepo.Setup(x => 
            x.AddBook(It.IsAny<Book>())).Throws<ArgumentNullException>();
        
        Assert.Throws<ArgumentNullException>(() => _libraryService.AddBook("Dune", ""));
    }
    
    [Test]
    public void DeleteBook_ValidArgument_DeleteSuccess()
    {
        _mockLibraryRepo.Setup(x => x.DeleteBook(It.IsAny<Book>())).Returns(_booklist[0]);
        
        var book = _libraryService.DeleteBook(_booklist[0]);
        Assert.That(book, Is.EqualTo(_booklist[0]));
    }
    
    [Test]
    public void DeleteBook_InvalidArgument_DeleteFailure()
    {
        _mockLibraryRepo.Setup(x => 
            x.AddBook(It.IsAny<Book>())).Throws<DbUpdateException>();
        
        Assert.Throws<DbUpdateException>(() => 
            _libraryService.AddBook("Animal Farm", "George Orwell"));
    }

    [Test]
    public void BorrowBook_ValidBook_BorrowSuccess()
    {
        var unborrowedBook = new Book("Dune", "Frank Herbert");
        
        var borrowedBook = new Book("Dune", "Frank Herbert");
        borrowedBook.BorrowBook();
        
        _mockLibraryRepo.Setup(x => 
            x.BorrowBook(It.IsAny<string>())).Returns(borrowedBook);
        
        var book = _libraryService.BorrowBook(unborrowedBook);
        
        Assert.That(book, Is.EqualTo(borrowedBook));
        Assert.That(book.IsBorrowed, Is.True);
    }
    
    [Test]
    public void BorrowBook_ValidBook_BorrowFailure()
    {
        var borrowedBook = _booklist[0];
        borrowedBook.BorrowBook();
        
        Assert.Throws<Exception>(() => _libraryService.BorrowBook(borrowedBook));
    }
    
    [Test]
    public void ReturnBook_ValidBook_ReturnSuccess()
    {
        var borrowedBook = _booklist[0];
        borrowedBook.BorrowBook();
        
        var returnedBook = new Book("Dune", "Frank Herbert");
        returnedBook.BorrowBook();
        returnedBook.ReturnBook();
        
        _mockLibraryRepo.Setup(x => 
            x.ReturnBook(It.IsAny<string>())).Returns(returnedBook);
        
        var book = _libraryService.ReturnBook(borrowedBook);
        Assert.That(book, Is.EqualTo(returnedBook));
        Assert.That(book.IsBorrowed, Is.False);
    }
    
    [Test]
    public void ReturnBook_ValidBook_ReturnFailure()
    {
        var borrowedBook = new Book("Dune", "Frank Herbert");
        
        Assert.Throws<Exception>(() => _libraryService.ReturnBook(borrowedBook));
    }
}