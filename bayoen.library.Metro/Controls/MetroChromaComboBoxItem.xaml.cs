using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bayoen.library.Metro.Controls
{
    public partial class MetroChromaComboBoxItem : ComboBoxItem
    {
        public MetroChromaComboBoxItem()
        {
            this.InitializeComponent();
        }

        public Brush IconForeground
        {
            get { return GetValue(IconForegroundProperty) as Brush; }
            set { SetValue(IconForegroundProperty, value); }
        }
        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
            "IconForeground",
            typeof(Brush),
            typeof(MetroChromaComboBoxItem));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MetroChromaComboBoxItem));
    }
}
