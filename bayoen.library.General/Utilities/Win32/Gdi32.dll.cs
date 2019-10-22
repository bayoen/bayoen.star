using System;
using System.Runtime.InteropServices;

namespace bayoen.library.General.Utilities.Win32
{
    public class Gdi32
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
