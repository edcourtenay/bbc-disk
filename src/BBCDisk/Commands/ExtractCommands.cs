using BBCDisk.Services;
using Cocona;

namespace BBCDisk.Commands;

public class ExtractCommands
{
    private readonly DiskHandler _diskHandler;

    public ExtractCommands(DiskHandler diskHandler)
    {
        _diskHandler = diskHandler;
    }
    
    public void ExtractFile([Option("disk", ['d'], Description = "Disk image to load")] string diskFilename, string file, string? destination)
    {
        var disk = _diskHandler.Read(diskFilename);

        if (!disk.TryReadFile(file, out var data))
        {
            Console.Error.WriteLine("Cannot locate file ${file} on disk image");
            return;
        }
        
        using var fs = new FileStream(destination ?? file, FileMode.Create, FileAccess.Write);
        using var sw = new BinaryWriter(fs);
            
        sw.Write(data.ToArray());
    }
}