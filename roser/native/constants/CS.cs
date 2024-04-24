namespace roser.native
{
    public enum CS : uint
    {
        VRedraw = 0x0001,
        HRedraw = 0x0002,
        Dblclks = 0x0008,
        OwnDc = 0x0020,
        ClassDc = 0x0040,
        ParentDc = 0x0080,
        Noclose = 0x0200,
        SaveBits = 0x0800,
        ByteAlignClient = 0x1000,
        ByteAlignWindow = 0x2000,
        GlobalClass = 0x4000,
        Ime = 0x00010000,
        DropShadow = 0x00020000,
    }
}
