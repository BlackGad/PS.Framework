using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace PS.Windows.Interop
{
    public static class Kernel32
    {
        #region Constants

        public const UInt32 StdOutputHandle = 0xFFFFFFF5;

        #endregion

        #region Static members

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(UInt32 nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeProcessHandle OpenProcess(int access, bool inherit, int processId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadFile(IntPtr handle,
                                           IntPtr buffer,
                                           uint numBytesToRead,
                                           out uint lpNumberOfBytesRead,
                                           IntPtr lpOverlapped);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void SetLastError(int dwErrorCode);

        [DllImport("kernel32.dll")]
        public static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(IntPtr handle,
                                            IntPtr buffer,
                                            uint numBytesToWrite,
                                            out uint lpNumberOfBytesWritten,
                                            IntPtr lpOverlapped);

        #endregion
    }
}