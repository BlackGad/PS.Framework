using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

// ReSharper disable InconsistentNaming

namespace PS.Windows.Interop.Components
{
    public class OpenFolderDialog : IDisposable
    {
        public static IWin32Window GetIWin32Window(Visual visual)
        {
            var source = (HwndSource)PresentationSource.FromVisual(visual);
            // ReSharper disable once PossibleNullReferenceException
            return new OldWindow(source.Handle);
        }

        /// <summary>
        /// Gets/sets directory in which dialog will be open if there is no recent directory available.
        /// </summary>
        public string DefaultFolder { get; set; }

        /// <summary>
        /// Gets/sets folder in which dialog will be open.
        /// </summary>
        public string InitialFolder { get; set; }

        /// <summary>
        /// Gets selected folder.
        /// </summary>
        public string SelectedFolder { get; private set; }

        public void Dispose()
        {
            //just to have possibility of Using statement.
        }

        public bool ShowDialog()
        {
            var result = ShowVistaDialog(null);
            return result == DialogBoxResult.OK;
        }

        public bool ShowDialog(Window owner)
        {
            var result = ShowVistaDialog(GetIWin32Window(owner));
            return result == DialogBoxResult.OK;
        }

        private DialogBoxResult ShowVistaDialog(IWin32Window owner)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var frm = (IFileDialog)new FileOpenDialogRCW();
            frm.GetOptions(out var options);
            options |= FileOpenDialogOptions.PickFolders |
                       FileOpenDialogOptions.ForceFileSystem |
                       FileOpenDialogOptions.NoValidate |
                       FileOpenDialogOptions.NoTestFileCreate |
                       FileOpenDialogOptions.DontAddToRecent;
            frm.SetOptions(options);
            if (InitialFolder != null)
            {
                var riid = typeof(IShellItem).GUID;
                if (Shell32.SHCreateItemFromParsingName(InitialFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == ErrorCode.S_OK)
                {
                    frm.SetFolder(directoryShellItem);
                }
            }

            if (DefaultFolder != null)
            {
                var riid = typeof(IShellItem).GUID;
                if (Shell32.SHCreateItemFromParsingName(DefaultFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == ErrorCode.S_OK)
                {
                    frm.SetDefaultFolder(directoryShellItem);
                }
            }

            var result = owner == null
                ? frm.Show()
                : frm.Show(owner.Handle);

            if (result == ErrorCode.S_OK &&
                frm.GetResult(out var shellItem) == ErrorCode.S_OK &&
                shellItem.GetDisplayName(SIGDN.FileSysPath, out var pszString) == ErrorCode.S_OK &&
                pszString != IntPtr.Zero)
            {
                try
                {
                    SelectedFolder = Marshal.PtrToStringAuto(pszString);
                    return DialogBoxResult.OK;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pszString);
                }
            }

            return DialogBoxResult.Cancel;
        }

        #region Nested type: OldWindow

        private class OldWindow : IWin32Window
        {
            private readonly IntPtr _handle;

            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            IntPtr IWin32Window.Handle
            {
                get { return _handle; }
            }
        }

        #endregion
    }
}
