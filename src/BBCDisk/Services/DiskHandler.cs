using System.Text;
using BBCDisk.Commands;
using BBCDisk.Models;
using BBCDisk.Services;
using Cocona;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<DiskHandler>();

var app = builder.Build();

app.AddCommands<CatCommands>();
app.AddCommands<DumpCommands>();

app.Run();

namespace BBCDisk.Services
{
    public class DiskHandler
    {
        public Disk Read(string filename)
        {
            var fullPath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), filename));

            Memory<byte> diskImage = File.ReadAllBytes(fullPath);

            var sector0 = ReadSector(diskImage, 0, 0);
            var sector1 = ReadSector(diskImage, 0, 1);

            ReadOnlySpan<byte> titleBytes = [..sector0.Span[..8], ..sector1.Span[..4]];
            var title = Encoding.ASCII.GetString(titleBytes);
            int cycle = 10 * (sector1.Span[4] & 0xF0 >> 4) + (sector1.Span[4] & 0x0F);

            int fileOffset = sector1.Span[5];
            int files = fileOffset / 8;

            var bootOption = (BootOption)(sector1.Span[6] >> 4);

            var diskSize = 256 * (sector1.Span[6] & 0x03) + sector1.Span[7];

            CatalogEntry[] catalogEntries = [..NewMethod(files, diskImage)];

            return new Disk
            {
                Title = title,
                Cycle = cycle,
                BootOption = bootOption,
                DiskSize = diskSize,
                CatalogEntries = catalogEntries
            };
        }

        private IEnumerable<CatalogEntry> NewMethod(int files, Memory<byte> diskImage)
        {
            for (int i = 0; i < files; i++)
            {
                yield return ReadCatalogEntry(diskImage, i);
            }
        }

        private CatalogEntry ReadCatalogEntry(Memory<byte> diskImage, int i)
        {
            var offset = i * 8 + 8;
            var sector0 = ReadSector(diskImage, 0, 0);
            var sector1 = ReadSector(diskImage, 0, 1);

            return new CatalogEntry
            {
                Filename = Encoding.ASCII.GetString(sector0.Span[offset..(offset + 7)].TrimEnd((byte)0x20)),
                Directory = (char)(sector0.Span[offset + 7] & 0x7F),
                Locked = (sector0.Span[offset + 7] & 0x80) == 0x80,
                LoadAddress = 0x10000 * ((sector1.Span[offset + 6] & 0xC) >> 2) + 0x100 * sector1.Span[offset + 1] + sector1.Span[offset],
                ExecAddress = 0x10000 * ((sector1.Span[offset + 6] & 0xC0) >> 6) + 0x100 * sector1.Span[offset + 3] + sector1.Span[offset + 2],
                Length = 0x10000 * ((sector1.Span[offset + 6] & 0x30) >> 4) + 0x100 * sector1.Span[offset + 5] + sector1.Span[offset + 4],
                StartSector = 0x100 * (sector1.Span[offset + 6] & 0x03) + sector1.Span[offset + 7]
            };
        }

        private static Memory<byte> ReadSector(Memory<byte> diskImage, int track, int sector) =>
            diskImage.Slice((track * 10 + sector) * 256, 256);
    }
}