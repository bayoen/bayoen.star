using System;
using System.Collections.Generic;
using System.Text;

namespace bayoen.library.General.Utilities.Win32
{
    /// <summary>
    /// A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values.
    /// </summary>
    public enum HWND : int
    {
        /// <summary>
        /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
        /// </summary>
        NOTOPMOST = -2,
        /// <summary>
        /// Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        TOPMOST,
        /// <summary>
        /// Places the window at the top of the Z order.
        /// </summary>
        TOP,
        /// <summary>
        /// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
        /// </summary>
        BOTTOM,
    }
}
