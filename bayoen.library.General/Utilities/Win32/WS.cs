using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.library.General.Utilities.Win32
{
    /// <summary>
    /// Current Window Styles
    /// </summary>
    public enum WS : long
    {
        OVERLAPPED = 0x00000000,
        MAXIMIZE = 0x01000000,
        DISABLED = 0x08000000,
        VISIBLE = 0x10000000,
        MINIMIZE = 0x20000000
    }
}
