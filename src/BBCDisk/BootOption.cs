namespace BBCDisk;

public enum BootOption
{
    None = 0b00,
    Load = 0b01,
    Run =  0b10,
    Exec = 0b11
}