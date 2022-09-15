using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PS.Windows.Interop
{
    public static class User32
    {
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [SecurityCritical]
        [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

        [SecurityCritical]
        public static int GetWindowLong(HandleRef hWnd, WindowLongValue nIndex)
        {
            return GetWindowLong(hWnd, nIndex, out _);
        }

        [SecurityCritical]
        public static int GetWindowLong(HandleRef hWnd, WindowLongValue nIndex, out int error)
        {
            int result;
            Kernel32.SetLastError(0);
            if (IntPtr.Size == 4)
            {
                result = IntGetWindowLong(hWnd, (int)nIndex);
                error = Marshal.GetLastWin32Error();
            }
            else
            {
                var resultPtr = IntGetWindowLongPtr(hWnd, (int)nIndex);
                error = Marshal.GetLastWin32Error();
                result = (int)resultPtr.ToInt64();
            }

            return result;
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool IsIconic(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern DialogBoxResult MessageBox(IntPtr hWnd, String text, String caption, MessageBoxStyle options);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(HandleRef hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(HandleRef hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);

        [SecurityCritical]
        public static void SetMenuItem(HandleRef hMenu, SystemMenu menu, bool isEnabled)
        {
            EnableMenuItem(hMenu, menu, isEnabled ? ~1 : 1);
        }

        [SecurityCritical]
        public static void SetSystemMenuItems(HandleRef hWnd, bool isEnabled, params SystemMenu[] menus)
        {
            if (menus != null && menus.Length > 0)
            {
                var hMenu = new HandleRef(null, GetSystemMenu(hWnd, false));

                foreach (var menu in menus)
                {
                    SetMenuItem(hMenu, menu, isEnabled);
                }
            }
        }

        [SecurityCritical]
        [SecuritySafeCritical]
        public static IntPtr SetWindowLong(HandleRef hWnd, WindowLongValue nIndex, IntPtr dwNewLong)
        {
            return SetWindowLong(hWnd, nIndex, dwNewLong, out _);
        }

        public static IntPtr SetWindowLong(HandleRef hWnd, WindowLongValue nIndex, IntPtr dwNewLong, out int error)
        {
            IntPtr result;
            Kernel32.SetLastError(0);
            if (IntPtr.Size == 4)
            {
                var intResult = IntSetWindowLong(hWnd, (int)nIndex, (int)dwNewLong.ToInt64());
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(intResult);
            }
            else
            {
                result = IntSetWindowLongPtr(hWnd, (int)nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            return result;
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, ShowState nCmdShow);

        [SecurityCritical]
        [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool EnableMenuItem(HandleRef hMenu, SystemMenu uidEnabledItem, int uEnable);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntGetWindowLong(HandleRef hWnd, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr IntGetWindowLongPtr(HandleRef hWnd, int nIndex);

        [DllImport("user32", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntSetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
    }
}
