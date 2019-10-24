using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.Metro.Controls;
using bayoen.library.Metro.Windows;
using bayoen.star.Controls;
using bayoen.star.Localizations;

namespace bayoen.star.Windows
{
    public partial class SettingWindow
    {
        private void BuildLanguageCodeComboBox()
        {
            double languageCodeComboBoxMaxWidth = -1;
            this.LanguageCodeComboBox.ItemSource = Config.CultureCodes.ConvertAll(x =>
            {
                CultureInfo info = new CultureInfo(x);
                TextBlock block = new TextBlock()
                {
                    Text = (info.NativeName == info.DisplayName) ? info.NativeName : $"{info.NativeName} ({info.DisplayName})",
                };
                block.SetResourceReference(TextBlock.ForegroundProperty, "BlackBrush");
                block.Arrange(new Rect(block.DesiredSize));
                languageCodeComboBoxMaxWidth = Math.Max(languageCodeComboBoxMaxWidth, block.ActualWidth);
                return block;
            });
            this.LanguageCodeComboBox.ComboBoxWidth = (languageCodeComboBoxMaxWidth == -1) ? 250 : languageCodeComboBoxMaxWidth + 40;
            this.LanguageCodeComboBox.SelectedIndex = Config.CultureCodes.IndexOf(Core.Option.LanguageCode);
        }
    }
}
