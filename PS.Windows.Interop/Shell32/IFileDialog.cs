using System;
using System.Runtime.InteropServices;

namespace PS.Windows.Interop
{
    [ComImport]
    [Guid("42F85136-DB7E-439C-85F1-E4075D135FC8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileDialog
    {
        [PreserveSig]
        ErrorCode Show([In][Optional] IntPtr hwndOwner);

        uint SetFileTypes([In] uint cFileTypes,
                          [In][MarshalAs(UnmanagedType.LPArray)] IntPtr rgFilterSpec);

        uint SetFileTypeIndex([In] uint iFileType);

        uint GetFileTypeIndex(out uint piFileType);

        uint Advise([In][MarshalAs(UnmanagedType.Interface)] IntPtr pfde,
                    out uint pdwCookie);

        uint Unadvise([In] uint dwCookie);

        uint SetOptions([In] FileOpenDialogOptions fos);

        uint GetOptions(out FileOpenDialogOptions fos);

        void SetDefaultFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);

        uint SetFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);

        uint GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        uint GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        uint SetFileName([In][MarshalAs(UnmanagedType.LPWStr)] string pszName);

        uint GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

        uint SetTitle([In][MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

        uint SetOkButtonLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszText);

        uint SetFileNameLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

        ErrorCode GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        uint AddPlace([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi,
                      uint fdap);

        uint SetDefaultExtension([In][MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

        uint Close([MarshalAs(UnmanagedType.Error)] uint hr);

        uint SetClientGuid([In] ref Guid guid);

        uint ClearClientData();

        uint SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
    }
}
