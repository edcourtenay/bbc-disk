using Cocona;

namespace BBCDisk.Arguments;

public record DiskArguments : ICommandParameterSet
{
    [Argument("disk", Description = "The disk image to handle")]
    public required string Disk { get; init; }
}