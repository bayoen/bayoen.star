using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace bayoen.library.Metro.Controls
{
    public partial class MetroButton : Button
    {
        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public new double FontSize
        {
            get => this.TextBlock.FontSize;
            set
            {
                if (this.TextBlock.FontSize == value) return;

                this.TextBlock.FontSize = value;
            }
        }

        public new FontWeight FontWeight
        {
            get => this.TextBlock.FontWeight;
            set
            {
                if (this.TextBlock.FontWeight == value) return;

                this.TextBlock.FontWeight = value;
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MetroButton));

        public MetroButton()
        {
            this.InitializeComponent();
            
            this.FontSize = 12;
            this.FontWeight = FontWeights.Bold;
        }
    }
}
