using library_management.Model;
using library_management.Services.IO;
using library_management.Services.Library;
using Moq;
using Moq.Protected;
using NUnit.Framework.Legacy;

namespace LibraryTesting.Service;

public class LibraryTesting
{
   private Mock<ILibraryService> _libraryServiceMock;
   private Library _library;
   private StringWriter _output;

   [SetUp]
   public void Setup()
   {
      _libraryServiceMock = new Mock<ILibraryService>();
      var outputWriter = new OutputWriter(
            new WelcomeMessages(),
            new ViewMessages(),
            new SearchMessages(),
            new AddMessages(),
            new DeleteMessages(),
            new BorrowMessages(),
            new ReturnMessages(), 
            new ErrorResponseMessages()
         );
      _library = new Library(_libraryServiceMock.Object, outputWriter);
      _output = new StringWriter();
      Console.SetOut(_output);
   }

   [TearDown]
   public void TearDown()
   {
      _output.Flush();
      _output.Close();
   }

   [
      TestCase("add\n"), 
      TestCase("Add new book\n"), 
      TestCase("delete\n"), 
      TestCase("search\n"),
      TestCase("borrow\n"),
      TestCase("return\n"),
   ]
   public void Run_PathTests_CorrectPathTriggered(string userInput)
   {
      _libraryServiceMock
         .Setup(l => l.FetchAvailableBooks())
         .Returns([new Book("test", "book")]);
      
      Console.SetIn(new StringReader($"{userInput}exit\nexit\n"));
      
      _library.Run();
      
      _libraryServiceMock.VerifyNoOtherCalls();
      Assert.That(_library.Running, Is.False);

   }
   
   [TestCase("view\n")] 
   public void Run_ViewTest_CorrectPathTriggered(string userInput)
   {
      _libraryServiceMock
         .Setup(l => l.FetchAvailableBooks())
         .Returns([new Book("test", "book")]);
      
      Console.SetIn(new StringReader($"{userInput}exit\nexit\n"));
      
      _library.Run();
      
      _libraryServiceMock.Verify(l => l.FetchAvailableBooks(), Times.Once);
      Assert.That(_library.Running, Is.False);

   }

   [TestCase("exit\n"), TestCase("quit\n")]
   public void Run_PathTests_ExitFunction(string userInput)
   {
      Console.SetIn(new StringReader(userInput));
      
      _library.Run();
      
      Assert.That(_library.Running, Is.False);
   }

   [Test]
   public void Run_PathTest_HandleUnexpectedInput()
   {
      Console.SetIn(new StringReader("unexpected\n\nexit\n"));

      _library.Run();
      
      Assert.That(
         _output.ToString(), 
         Does.Contain("Command not recognised"));
      _libraryServiceMock.VerifyNoOtherCalls();
   }
}