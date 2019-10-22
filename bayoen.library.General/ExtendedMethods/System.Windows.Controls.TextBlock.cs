using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static bool SetResourceFormattedText(this TextBlock block, string key, params object[] args)
        {
            try
            {
                string seedString = block.TryFindResource(key) as string;
                block.Text = string.Format(seedString, args);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
