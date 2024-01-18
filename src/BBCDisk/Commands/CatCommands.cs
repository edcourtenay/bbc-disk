using System.Text;
using Cocona;

namespace BBCDisk.Commands;

public class CatCommands
{
    [Command("cat")]
    public void Cat([Argument] string filename)
    {
        var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));
        Console.WriteLine($"Cat {fullPath} {File.Exists(fullPath)}");

        Memory<byte> diskImage = File.ReadAllBytes(fullPath);

        var sector0 = ReadSector(diskImage, 0, 0);
        var sector1 = ReadSector(diskImage, 0, 1);

        ReadOnlySpan<byte> title = [..sector0.Span[..8], ..sector1.Span[..4]];
        Console.WriteLine($"Title: {Encoding.ASCII.GetString(title)}");

        int cycle = 10 * (sector1.Span[4] & 0xF0 >> 4) + (sector1.Span[4] & 0x0F);
        Console.WriteLine($"Cycle: {cycle:x2}");

        int fileOffset = sector1.Span[5];
        int files = fileOffset / 8;
        Console.WriteLine($"File Offset: {fileOffset}");

        var bootOption = (BootOption)(sector1.Span[6] >> 4);
        Console.WriteLine($"Boot Option: {bootOption}");

        var diskSize = 256 * (sector1.Span[6] & 0x03) + sector1.Span[7];
        Console.WriteLine($"Disk Size: {diskSize}");

        for (int i = 0; i < files; i++)
        {
            var catalogEntry = ReadCatalogEntry(diskImage, i);
            Console.WriteLine($"Catalog Entry {i,2}: {catalogEntry}");
        }
    }

    private CatalogEntry ReadCatalogEntry(Memory<byte> diskImage, int i)
    {
        var offset = i * 8 + 8;
        var sector0 = ReadSector(diskImage, 0, 0);
        var sector1 = ReadSector(diskImage, 0, 1);

        return new CatalogEntry
        {
            Filename = Encoding.ASCII.GetString(sector0.Span[offset..(offset + 7)].TrimEnd((byte)0x20)),
            Directory = (char)(sector0.Span[offset + 7] & 0x7F),
            Locked = (sector0.Span[offset + 7] & 0x80) == 0x80,
            LoadAddress = 0x10000 * ((sector1.Span[offset + 6] & 0xC) >> 2) + 0x100 * sector1.Span[offset + 1] + sector1.Span[offset],
            ExecAddress = 0x10000 * ((sector1.Span[offset + 6] & 0xC0) >> 6) + 0x100 * sector1.Span[offset + 3] + sector1.Span[offset + 2],
            Length = 0x10000 * ((sector1.Span[offset + 6] & 0x30) >> 4) + 0x100 * sector1.Span[offset + 5] + sector1.Span[offset + 4],
            StartSector = 0x100 * (sector1.Span[offset + 6] & 0x03) + sector1.Span[offset + 7]
        };
    }


    private static Memory<byte> ReadSector(Memory<byte> diskImage, int track, int sector) =>
        diskImage.Slice((track * 10 + sector) * 256, 256);
}