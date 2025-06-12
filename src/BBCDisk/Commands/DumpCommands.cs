using System.Text;
using BBCDisk.Services;
using Cocona;

namespace BBCDisk.Commands;

public class DumpCommands
{
    private readonly DiskHandler _diskHandler;

    public DumpCommands(DiskHandler diskHandler)
    {
        _diskHandler = diskHandler;
    }

    [Command("dumpfile")]
    public void DumpText([Option("disk", ['d'], Description = "Disk image to load")] string diskFilename, string file, bool text = false)
    {
        var disk = _diskHandler.Read(diskFilename);

        if (!disk.TryReadFile(file, out var data))
        {
            Console.Error.WriteLine("Cannot locate file ${file} on disk image");
            return;
        }

        switch (text)
        {
            case true:
                DumpText(data);
                break;
            default:
                DumpHex(data);
                break;
        }
    }

    [Command("dumpsector")]
    public void DumpSector([Option("disk", ['d'], Description = "Disk image to load")] string diskFilename, int sector, int count = 1)
    {
        var disk = _diskHandler.Read(diskFilename);

        var memory = disk.ReadSector(sector, count);

        DumpHex(memory);
    }

    private static void DumpText(Memory<byte> memory)
    {
        var destination = new Span<byte>(new byte[memory.Length]);
        memory.Span.CopyTo(destination);

        destination.Replace((byte)'\r', (byte)'\n');
        Console.WriteLine(Encoding.ASCII.GetString(destination));
    }

    private static void DumpHex(Memory<byte> memory)
    {
        const int chunkSize = 16;
        foreach (var (bytes, offset) in memory.Span.ToArray().Chunk(chunkSize).Select((bytes, i) => (bytes, i * chunkSize)))
        {
            var col1 = new StringBuilder();
            var col2 = new StringBuilder();

            foreach (var b in bytes)
            {
                var c = (char)b;

                col1.Append($"{b:X2} ");
                col2.Append(char.IsControl(c) ? '.' : c);
            }
            
            Console.WriteLine($"{offset:X4}: {col1, -chunkSize * 3} {col2, -chunkSize}");
        }
    }
}