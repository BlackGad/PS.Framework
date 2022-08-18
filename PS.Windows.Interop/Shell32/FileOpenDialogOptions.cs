using System;

namespace PS.Windows.Interop
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/ne-shobjidl_core-_fileopendialogoptions
    /// </summary>
    [Flags]
    public enum FileOpenDialogOptions : uint
    {
        OverwritePrompt = 0x00000002,
        StrictFileTypes = 0x00000004,
        NoChangeDir = 0x00000008,
        PickFolders = 0x00000020,
        ForceFileSystem = 0x00000040,
        AllNonStorageItems = 0x00000080,
        NoValidate = 0x00000100,
        AllowMultiSelect = 0x00000200,
        PathMustExist = 0x00000800,
        FileMustExist = 0x00001000,
        CreatePrompt = 0x00002000,
        ShareAware = 0x00004000,
        NoReadonlyReturn = 0x00008000,
        NoTestFileCreate = 0x00010000,
        HideMruPlaces = 0x00020000,
        HidePinnedPlaces = 0x00040000,
        NoDereferenceLinks = 0x00100000,
        OkButtonNeedsInteraction = 0x00200000,
        DontAddToRecent = 0x02000000,
        ForceShowHidden = 0x10000000,
        DefaultNoMiniMode = 0x20000000,
        ForcePreviewPaneOn = 0x40000000,
        SupportStreamableItems = 0x80000000
    }
}
