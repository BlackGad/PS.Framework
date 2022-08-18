using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PS.Windows.Interop.Extensions
{
    public static class ProcessExtensions
    {
        public static void BringMainWindowToFront(this Process process)
        {
            if (process == null) return;
            // get the window handle
            var windowRef = new HandleRef(process, process.MainWindowHandle);
            // if iconic, we need to restore the window
            if (User32.IsIconic(windowRef)) User32.ShowWindowAsync(windowRef, ShowState.Restore);
            // bring it to the foreground
            User32.SetForegroundWindow(windowRef);
        }
    }
}
