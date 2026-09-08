namespace library_management.Services.IO;

public class InputReader
{
    public string Read()
    {
        return Console.ReadLine() ?? throw new Exception("Input was null");
    }
}