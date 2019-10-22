using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.library.General.Utilities.Win32
{
    /// <summary>
    /// 'Rect' struct for 'user32.dll'
    /// </summary>
    public struct RECT
    {
        /// <summary>
        /// x position of upper-left corner
        /// </summary>
        public int Left;

        /// <summary>
        /// y position of upper-left corner
        /// </summary>
        public int Top;

        /// <summary>
        /// x position of lower-right corner
        /// </summary>
        public int Right;

        /// <summary>
        /// y position of lower-right corner
        /// </summary>
        public int Bottom;

        public int Width
        {
            get => this.Right - this.Left;
        }
        public int Height
        {
            get => this.Bottom - this.Top;
        }

        public RECT(RECT rect)
        {
            this.Left = rect.Left;
            this.Top = rect.Top;
            this.Right = rect.Right;
            this.Bottom = rect.Bottom;
        }
    }
}
