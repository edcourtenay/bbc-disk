using Cocona;

namespace BBCDisk.Commands;

public class CatCommands
{
    [Command("cat")]
    public void Cat([Argument] string filename)
    {
        Console.WriteLine($"Cat {filename}");
    }
}