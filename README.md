# bbc-disk

A utility to manage virtual BBC Micro disk images.

## Features

- List files in a BBC Micro disk image
- Extract files from a disk image

## Usage

Run the utility from the command line:

```
bbc-disk <command> [options]
```

### Commands

- `cat <image.ssd>`

Performs a `cat` operation on the disk image, listing its contents.

```
$ bbc-disk cat --disk elite.ssd
Title: ELITE128TUBE
Cycle: 63
Boot Option: Exec
Disk Size: 800
D.MOP       005600 005600 000A00 2B3
D.MOO       005600 005600 000A00 2A9
D.MON       005600 005600 000A00 29F
D.MOM       005600 005600 000A00 295
D.MOL       005600 005600 000A00 28B
D.MOK       005600 005600 000A00 281
D.MOJ       005600 005600 000A00 277
D.MOI       005600 005600 000A00 26D
D.MOH       005600 005600 000A00 263
D.MOG       005600 005600 000A00 259
D.MOF       005600 005600 000A00 24F
D.MOE       005600 005600 000A00 245
D.MOD       005600 005600 000A00 23B
D.MOC       005600 005600 000A00 231
D.MOB       005600 005600 000A00 227
D.MOA       005600 005600 000A00 21D
D.CODE      0011E3 0011E3 00441D 1D8
T.CODE      0011E3 0011E3 004E1D 189
$.M128Elt   FF0E00 FF0E43 0002D1 186
I.ELITEa    FF2000 FF2000 001689 16F
I.CODE      FF2400 FF2C89 001936 155
$.BDATA     000000 000000 004200 113
$.ELPIC     FF0900 FF0900 00016A 111
$.TubeElt   FF2000 FF2085 0002F0 10E
$.BCODE     000000 000000 006C48 0A1
P.CODE      001000 00106A 00978F 009
E.A         000000 000000 000100 008
$.ELITE     FF1900 FF8023 000193 006
$.LOAD      FF1900 FF8023 00010E 004
$.!BOOT     000000 FFFFFF 00000E 003
E.A1        000000 000000 000100 002
```
- `dumpsector --disk <image.ssd> --sector <sector> [--count <count>]`

Dumps the specified sector(s) from the disk image. For example, to dump the catalog area of a disk image:

```
$ bbc-disk dumpsector --disk elite.ssd --sector 0 --count 2
0000: 45 4C 49 54 45 31 32 38 4D 4F 50 20 20 20 20 44  ELITE128MOP    D
0010: 4D 4F 4F 20 20 20 20 44 4D 4F 4E 20 20 20 20 44  MOO    DMON    D
0020: 4D 4F 4D 20 20 20 20 44 4D 4F 4C 20 20 20 20 44  MOM    DMOL    D
0030: 4D 4F 4B 20 20 20 20 44 4D 4F 4A 20 20 20 20 44  MOK    DMOJ    D
0040: 4D 4F 49 20 20 20 20 44 4D 4F 48 20 20 20 20 44  MOI    DMOH    D
0050: 4D 4F 47 20 20 20 20 44 4D 4F 46 20 20 20 20 44  MOG    DMOF    D
0060: 4D 4F 45 20 20 20 20 44 4D 4F 44 20 20 20 20 44  MOE    DMOD    D
0070: 4D 4F 43 20 20 20 20 44 4D 4F 42 20 20 20 20 44  MOC    DMOB    D
0080: 4D 4F 41 20 20 20 20 44 43 4F 44 45 20 20 20 44  MOA    DCODE   D
0090: 43 4F 44 45 20 20 20 54 4D 31 32 38 45 6C 74 24  CODE   TM128Elt$
00A0: 45 4C 49 54 45 61 20 49 43 4F 44 45 20 20 20 49  ELITEa ICODE   I
00B0: 42 44 41 54 41 20 20 24 45 4C 50 49 43 20 20 24  BDATA  $ELPIC  $
00C0: 54 75 62 65 45 6C 74 24 42 43 4F 44 45 20 20 24  TubeElt$BCODE  $
00D0: 43 4F 44 45 20 20 20 50 41 20 20 20 20 20 20 45  CODE   PA      E
00E0: 45 4C 49 54 45 20 20 24 4C 4F 41 44 20 20 20 24  ELITE  $LOAD   $
00F0: 21 42 4F 4F 54 20 20 24 41 31 20 20 20 20 20 45  !BOOT  $A1     E
0100: 54 55 42 45 39 F8 33 20 00 56 00 56 00 0A 02 B3  TUBE9ø3 .V.V...³
0110: 00 56 00 56 00 0A 02 A9 00 56 00 56 00 0A 02 9F  .V.V...©.V.V....
0120: 00 56 00 56 00 0A 02 95 00 56 00 56 00 0A 02 8B  .V.V.....V.V....
0130: 00 56 00 56 00 0A 02 81 00 56 00 56 00 0A 02 77  .V.V.....V.V...w
0140: 00 56 00 56 00 0A 02 6D 00 56 00 56 00 0A 02 63  .V.V...m.V.V...c
0150: 00 56 00 56 00 0A 02 59 00 56 00 56 00 0A 02 4F  .V.V...Y.V.V...O
0160: 00 56 00 56 00 0A 02 45 00 56 00 56 00 0A 02 3B  .V.V...E.V.V...;
0170: 00 56 00 56 00 0A 02 31 00 56 00 56 00 0A 02 27  .V.V...1.V.V...'
0180: 00 56 00 56 00 0A 02 1D E3 11 E3 11 1D 44 01 D8  .V.V....ã.ã..D.Ø
0190: E3 11 E3 11 1D 4E 01 89 00 0E 43 0E D1 02 CD 86  ã.ã..N....C.Ñ.Í.
01A0: 00 20 00 20 89 16 CD 6F 00 24 89 2C 36 19 CD 55  . . ..Ío.$.,6.ÍU
01B0: 00 00 00 00 00 42 01 13 00 09 00 09 6A 01 CD 11  .....B......j.Í.
01C0: 00 20 85 20 F0 02 CD 0E 00 00 00 00 48 6C 00 A1  . . ð.Í.....Hl.¡
01D0: 00 10 6A 10 8F 97 00 09 00 00 00 00 00 01 00 08  ..j.............
01E0: 00 19 23 80 93 01 CC 06 00 19 23 80 0E 01 CC 04  ..#...Ì...#...Ì.
01F0: 00 00 FF FF 0E 00 C0 03 00 00 00 00 00 01 00 02  ..ÿÿ..À.........
```
- `dumpfile --disk <image.ssd> --file <filename>`

Dumps the specified file from the disk image. For example, to dump the file `$.!BOOT`:

```
$ bbc-disk dumpfile --disk elite.ssd --file !BOOT
0000: 2A 42 2E 0D 43 48 2E 22 4C 4F 41 44 22 0D        *B..CH."LOAD".
```
Note: Unless specified, the default directory (`$`) is assumed

The `--text` flag can be used to dump the file in a human-readable format:

```
$ bbc-disk dumpfile --disk elite.ssd --file !BOOT --text
*B.
CH."LOAD"
```
- `extractfile --disk <image.ssd> --file <filename> [--destination <output_file>]`

Extracts the specified file from the disk image to a local file. For example, to extract the file `$.!BOOT`:

```
bbc-disk extractfile --disk elite.ssd --file !BOOT
```

## License

MIT License. See `LICENSE` for details.
