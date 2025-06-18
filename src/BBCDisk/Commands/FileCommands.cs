using BBCDisk.Arguments;
using BBCDisk.Services;
using Cocona;

namespace BBCDisk.Commands;

public class FileCommands
{
    private readonly DiskHandler _diskHandler;

    public FileCommands(DiskHandler diskHandler)
    {
        _diskHandler = diskHandler;
    }
    
    [Command("extractfile")]
    public void ExtractFile(DiskArguments diskArgs, string file, string? destination)
    {
        _diskHandler.Read(diskArgs.Disk, diskImage =>
        {
            diskImage.ReadFile(file, data =>
            {
                File.WriteAllBytes(destination ?? file, data.ToArray());
            });
        });
    }
}