using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace bayoen.library.Metro.Controls
{
    public partial class WindowCommandButton : Button
    {
        public UIElement Icon
        {
            get { return GetValue(IconProperty) as UIElement; }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(UIElement),
            typeof(WindowCommandButton));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(WindowCommandButton));

        public new double FontSize
        {
            get => this.TextBlock.FontSize;
            set
            {
                if (this.TextBlock.FontSize == value) return;

                this.TextBlock.FontSize = value;
            }
        }

        public WindowCommandButton()
        {
            this.InitializeComponent();            
            this.FontSize = 12;
        }
    }
}
