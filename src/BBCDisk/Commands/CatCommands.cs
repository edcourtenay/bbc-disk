using BBCDisk.Services;
using Cocona;

namespace BBCDisk.Commands;

public class CatCommands
{
    private readonly DiskHandler _diskHandler;

    public CatCommands(DiskHandler diskHandler)
    {
        _diskHandler = diskHandler;
    }

    [Command("cat")]
    public void Cat([Option("disk", ['d'])] string filename)
    {
        var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));
        Console.WriteLine($"Cat {fullPath} {File.Exists(fullPath)}");

        var disk = _diskHandler.Read(filename);

        Console.WriteLine($"Title: {disk.Title}");
        Console.WriteLine($"Cycle: {disk.Cycle:x2}");
        //Console.WriteLine($"File Offset: {fileOffset}");

        Console.WriteLine($"Boot Option: {disk.BootOption}");
        Console.WriteLine($"Disk Size: {disk.DiskSize}");

        foreach (var catalogEntry in disk.CatalogEntries)
        {
            Console.WriteLine($"{catalogEntry}");
        }
    }
}