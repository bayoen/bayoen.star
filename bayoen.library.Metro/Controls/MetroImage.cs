using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace bayoen.library.Metro.Controls
{
    public class MetroImage : Image
    {
        public MetroImage()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
        }
    }
}
