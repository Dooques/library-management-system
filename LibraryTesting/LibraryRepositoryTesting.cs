using System.Diagnostics;
using library_management.Model;
using library_management.Services.Library;
using Moq;

namespace LibraryTesting;

public class LibraryServiceTesting
{
    private Mock<ILibraryRepository> _mockLibraryRepo;
    private LibraryService _libraryService;

    private List<Book> _booklist =
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
    public void ViewBooks_ValidBooks_ShouldReturnAllBooks()
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
        _mockLibraryRepo.Setup(x => 
            x.GetBooks()).Returns(_booklist);
        _libraryService.ViewBooks();
        
        Assert.That();
    }
}