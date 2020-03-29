namespace PS.Windows.Interop
{
    public enum WindowMessage
    {
        Destroy = 0x2,
        Close = 0x10,
        SetIcon = 0x80,
        MeasureItem = 0x2c,
        MouseMove = 0x200,
        MouseDown = 0x201,
        LButtonUp = 0x0202,
        LButtonDblClk = 0x0203,
        RButtonDown = 0x0204,
        RButtonUp = 0x0205,
        RButtonDblClk = 0x0206,
        MButtonDown = 0x0207,
        MButtonUp = 0x0208,
        MButtonDblClk = 0x0209,
        TrayMouseMessage = 0x800
    }
}