using library_management.Services.IO;

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