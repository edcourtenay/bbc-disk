using BBCDisk.Models;


namespace BBCDisk.Services;

public class DiskHandler
{
    public void Read(string filename, Action<Disk> action)
    {
        var path = Path.GetRelativePath(Directory.GetCurrentDirectory(), filename);

        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"Cannot locate file {filename}");
            return;
        }

        var disk = new Disk
        {
            Data = (Memory<byte>)File.ReadAllBytes(path)
        };

        action.Invoke(disk);
    }
}