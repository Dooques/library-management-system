namespace library_management.Services.IO;

public static class InputReader
{
    public static string ReadInput()
    {
        return Console.ReadLine() ?? throw new Exception("Input was null");
    }
}