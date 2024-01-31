namespace BBCDisk.Models;

public record Disk
{
    public string Title { get; init; }
    public int Cycle { get; init; }
    public BootOption BootOption { get; init; }
    public int DiskSize { get; init; }
    public CatalogEntry[] CatalogEntries { get; init; }
}