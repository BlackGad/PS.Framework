using System;
using System.Runtime.InteropServices;

namespace PS.Windows.Interop
{
    public static class Shell32
    {
        #region Static members

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern ErrorCode SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath,
                                                                   IntPtr pbc,
                                                                   ref Guid riid,
                                                                   [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        #endregion
    }
}