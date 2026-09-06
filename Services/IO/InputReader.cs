namespace library_management.Services.IO;

public static class InputReader
{
    public static string Read()
    {
        return Console.ReadLine() ?? throw new Exception("Input was null");
    }
}