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
    public enum WS : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_MAXIMIZE = 0x01000000,
        WS_DISABLED = 0x08000000,
        WS_VISIBLE = 0x10000000,
        WS_MINIMIZE = 0x20000000,
    }
}
