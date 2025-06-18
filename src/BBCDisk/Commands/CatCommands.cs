using System.Text;
using BBCDisk.Arguments;
using BBCDisk.Models;
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

    [Command("info", Description = "Show file information for a BBC Micro DFS disk image")]
    public void Info(DiskArguments diskArgs)
    {
        var filename = diskArgs.Disk;
        _diskHandler.Read(filename, disk =>
        {
            foreach (var catalogEntry in disk.CatalogEntries)
            {
                Console.WriteLine($"{catalogEntry}");
            }
        });
    }

    [Command("cat", Description = "Catalog a BBC Micro DFS disk image")]
    public void Cat(DiskArguments diskArgs)
    {
        _diskHandler.Read(diskArgs.Disk, disk =>
        {
            Console.WriteLine($"{disk.Title} ({disk.Cycle}) FM");
            
            var drive = 0;
            var dir = '$';

            var dirString = $"Dir. :{drive}.{dir}";
            var libString = $"Lib. :{drive}.{dir}";
            
            Console.WriteLine($"{$"Drive {drive}", -20}{$"Option {(int)disk.BootOption} ({disk.BootOption.ToString().ToUpper()})", -20}");
            Console.WriteLine($"{dirString, -20}{libString, -20}");
            Console.WriteLine();

            var defaultDirectory = disk
                .CatalogEntries.Where(entry => entry.Directory == dir);

            var others = disk
                .CatalogEntries.Where(entry => entry.Directory != dir);
            
            Console.Write(Temp(defaultDirectory, dir));
            Console.WriteLine();
            Console.Write(Temp(others, dir));
        });
    }

    private static string Temp(IEnumerable<CatalogEntry> defaultDirectory, char dir)
    {
        var chunked = defaultDirectory
            .OrderBy(entry => entry.Filename)
            .Chunk(2);
        
        var sb = new StringBuilder();
        foreach (var line in chunked)
        {
            foreach (var entry in line)
            {
                sb.Append($"{(entry.Directory == dir ? "" : $"{entry.Directory}."),4}");
                sb.Append($"{$"{entry.Filename,-8} {(entry.Locked ? 'L' : ' ')}",-20}");
            }

            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}