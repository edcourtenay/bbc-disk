namespace BBCDisk.Models;

public record CatalogEntry
{
    private readonly int _loadAddress;
    private readonly int _execAddress;
    public required string Filename { get; init; }
    public required char Directory { get; init; } = '$';
    public required bool Locked { get; init; }

    public required int LoadAddress
    {
        get => _loadAddress;
        init => _loadAddress = Address(value);
    }

    public required int ExecAddress
    {
        get => _execAddress;
        init => _execAddress = Address(value);
    }

    public required int Length { get; init; }
    public required int StartSector { get; init; }

    private int Address(int value) => (value & 0x030000) == 0x030000 ? value | 0xFF0000 : value;

    public override string ToString() =>
        $"{Directory}.{Filename,-7} {(Locked ? 'L' : ' ')} {LoadAddress:X6} {ExecAddress:X6} {Length:X6} {StartSector:X3}";
}