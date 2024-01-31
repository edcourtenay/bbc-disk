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

    public void DumpText([Option("disk", ['d'])] string filename)
    {

    }
}