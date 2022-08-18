using System;
using System.Runtime.InteropServices;

namespace PS.Windows.Interop
{
    [ComImport]
    [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        uint BindToHandler([In] IntPtr pbc,
                           [In] ref Guid rbhid,
                           [In] ref Guid riid,
                           [Out][MarshalAs(UnmanagedType.Interface)] out IntPtr ppvOut);

        uint GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        ErrorCode GetDisplayName([In] SIGDN sigdnName, out IntPtr ppszName);

        uint GetAttributes([In] uint sfgaoMask, out uint gaoAttributes);

        uint Compare([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi,
                     [In] uint hint,
                     out int piOrder);
    }
}
