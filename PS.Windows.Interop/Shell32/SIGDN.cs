// ReSharper disable InconsistentNaming

namespace PS.Windows.Interop
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/ne-shobjidl_core-sigdn
    /// </summary>
    public enum SIGDN : uint
    {
        NormalDisplay = 0x00000000,
        ParentRelativeParsing = 0x80018001,
        DesktopAbsoluteParsing = 0x80028000,
        ParentRelativeEditing = 0x80031001,
        DesktopAbsoluteEditing = 0x8004c000,
        FileSysPath = 0x80058000,
        URL = 0x80068000,
        ParentRelativeForAddressBar = 0x8007c001,
        ParentRelative = 0x80080001,
        ParentRelativeForUI = 0x80094001
    }
}