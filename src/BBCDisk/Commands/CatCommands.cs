using Cocona;

namespace BBCDisk.Commands;

public class CatCommands
{
    [Command("cat")]
    public void Cat([Argument] string filename)
    {
        var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));
        Console.WriteLine($"Cat {fullPath} {File.Exists(fullPath)}");
    }
}