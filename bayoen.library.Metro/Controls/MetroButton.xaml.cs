using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace bayoen.library.Metro.Controls
{
    public partial class MetroButton : Button
    {
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(MetroButton));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MetroButton));

        public UIElement Icon
        {
            get { return GetValue(IconProperty) as UIElement; }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(UIElement),
            typeof(MetroButton));

        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize",
            typeof(double),
            typeof(MetroButton));

        public new FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight",
            typeof(FontWeight),
            typeof(MetroButton));


        private bool _isAccented;
        public bool IsAccented
        {
            get => this._isAccented;
            set
            {
                if (this._isAccented == value) return;

                if (value)
                {
                    this.SetResourceReference(Control.StyleProperty, "AccentedSquareButtonStyle");
                }
                else
                {
                    this.SetResourceReference(Control.StyleProperty, "SquareButtonStyle");
                }

                this._isAccented = value;
            }
        }

        public MetroButton()
        {
            this.InitializeComponent();

            this._isAccented = false;


            this.FontSize = 18;
            this.FontWeight = FontWeights.Bold;
        }
    }
}
