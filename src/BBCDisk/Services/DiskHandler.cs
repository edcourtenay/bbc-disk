using BBCDisk.Models;


namespace BBCDisk.Services
{
    public class DiskHandler
    {
        public Disk Read(string filename)
        {
            var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));

            Memory<byte> diskImage = File.ReadAllBytes(fullPath);

            return new Disk
            {
                Data = diskImage
            };
        }
    }
}