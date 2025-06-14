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
    public void ExtractFile([Option(['d'], Description = "Disk image to load")] string disk, string file, string? destination)
    {
        _diskHandler.Read(disk, diskImage =>
        {
            diskImage.ReadFile(file, data =>
            {
                File.WriteAllBytes(destination ?? file, data.ToArray());
            });
        });
    }
}