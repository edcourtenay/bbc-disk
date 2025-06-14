using BBCDisk.Models;


namespace BBCDisk.Services
{
    public class DiskHandler
    {
        public Disk Read(string filename, Action<Disk>? action = null)
        {
            var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));

            var disk = new Disk
            {
                Data = (Memory<byte>)File.ReadAllBytes(fullPath)
            };
            action?.Invoke(disk);

            return disk;
        }
    }
}