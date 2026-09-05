using library_management.Model;
using library_management.Services.Database;
using library_management.Services.IO;
using Microsoft.EntityFrameworkCore;

using var dbContext = new LibraryContext();
var library = new Library(dbContext);

var running = true;
while (running)
{ 
    OutputWriter.WelcomeMessage();
    var userInput = InputReader.ReadInput();
    
    if  (userInput == "exit")
    {
        running = false;
    }
}