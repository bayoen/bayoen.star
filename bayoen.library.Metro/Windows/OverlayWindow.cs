using System;
using System.Collections.Generic;
using System.Text;

using MahApps.Metro.Controls;

namespace bayoen.library.Metro.Windows
{
    /// <summary>
    /// Basis overlay window for project bayoen from '<see cref="MetroWindow"/>'
    /// </summary>
    public class OverlayWindow : MetroWindow
    {
        public OverlayWindow()
        {
            this.BorderThickness = new System.Windows.Thickness(0);
            this.ShowTitleBar = false;
            this.TitlebarHeight = 0;

            this.IsCapturable = false;
        }

        public bool IsCapturable { get; set; }

    }
}
