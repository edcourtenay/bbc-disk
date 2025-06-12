using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BBCDisk.Models;

public record Disk
{
    public string Title
    {
        get
        {
            var catalogData = CatalogData();

            return Encoding.ASCII.GetString([..catalogData.Span[..8], ..catalogData.Span[256..260]]);
        }
    }

    public int Cycle
    {
        get
        {
            var catalogData = CatalogData();

            return 10 * (catalogData.Span[260] & 0xF0 >> 4) + (catalogData.Span[260] & 0x0F);
        }
    }

    public BootOption BootOption
    {
        get
        {
            var catalogData = CatalogData();

            return (BootOption)(catalogData.Span[262] >> 4);
        }
    }

    public int DiskSize
    {
        get
        {
            var sector1 = CatalogData();

            return 256 * (sector1.Span[262] & 0x03) + sector1.Span[263];
        }
    }

    public int FileOffset
    {
        get
        {
            var sector1 = CatalogData();

            return sector1.Span[261];
        }
    }

    public int FileCount => FileOffset / 8;

    public IEnumerable<CatalogEntry> CatalogEntries
    {
        get
        {
            for (int i = 0; i < FileCount; i++)
            {
                yield return ParseCatalogEntry(i);
            }
        }
    }

    public Memory<byte> Data { get; init; }

    public Memory<byte> ReadSector(int sector, int length) =>
        Data.Slice(sector * 256, 256 * length);

    public Memory<byte> ReadSector(int track, int sector, int length) =>
        Data.Slice((track * 10 + sector) * 256, 256 * length);
  
    public Memory<byte>? ReadFile(string filename)
    {
        return TryReadFile(filename, out var file) ? file : null;
    }
    
    public bool TryReadFile(string filename, [NotNullWhen(true)] out Memory<byte> file)
    {
        if (filename is not [_, '.', ..])
        {
            filename = $"$.{filename}";
        }

        var catalogEntry = CatalogEntries.FirstOrDefault(x => x.DirectoryFilename == filename);

        if (catalogEntry is not null)
        {
            file = ReadCatalogEntry(catalogEntry);
            return true;
        }

        file = null;
        return false;
    }

    public Memory<byte> ReadCatalogEntry(CatalogEntry catalogEntry) => 
        ReadSector(catalogEntry.StartSector, (catalogEntry.Length / 256) + 1)[..catalogEntry.Length];

    private Memory<byte> CatalogData() =>
        ReadSector(0, 0, 2);

    private CatalogEntry ParseCatalogEntry(int i)
    {
        var offset = i * 8 + 8;
        var catalogData = CatalogData();

        return new CatalogEntry
        {
            Filename = Encoding.ASCII.GetString(catalogData.Span[offset..(offset + 7)].TrimEnd((byte)0x20)),
            Directory = (char)(catalogData.Span[offset + 7] & 0x7F),
            Locked = (catalogData.Span[offset + 7] & 0x80) == 0x80,
            LoadAddress = 0x10000 * ((catalogData.Span[offset + 262] & 0xC) >> 2) + 0x100 * catalogData.Span[offset + 257] + catalogData.Span[offset + 256],
            ExecAddress = 0x10000 * ((catalogData.Span[offset + 262] & 0xC0) >> 6) + 0x100 * catalogData.Span[offset + 259] + catalogData.Span[offset + 258],
            Length = 0x10000 * ((catalogData.Span[offset + 262] & 0x30) >> 4) + 0x100 * catalogData.Span[offset + 261] + catalogData.Span[offset + 260],
            StartSector = 0x100 * (catalogData.Span[offset + 262] & 0x03) + catalogData.Span[offset + 263]
        };
    }
}