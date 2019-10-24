using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.library.General.Utilities.Win32
{
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the values of '<see cref="HWND"/>'</param>
        /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="y">The new position of the top of the window, in client coordinates.</param>
        /// <param name="width">The new width of the window, in pixels.</param>
        /// <param name="height">The new height of the window, in pixels.</param>
        /// <param name="uFlags">The window sizing and positioning flags. This parameter can be a combination of the values of '<see cref="SWP"/>'</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rectangle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}
