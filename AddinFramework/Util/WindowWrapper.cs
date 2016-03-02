using System;

namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Wrapper needed to be able to open properties dialog in EA
    /// </summary>
    internal class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle => _hwnd;

        private readonly IntPtr _hwnd;
    }
}
