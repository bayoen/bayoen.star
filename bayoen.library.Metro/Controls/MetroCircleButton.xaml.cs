using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace bayoen.library.Metro.Controls
{
    public partial class MetroCircleButton : Button
    {
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(MetroCircleButton));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MetroCircleButton));

        public UIElement Icon
        {
            get { return GetValue(IconProperty) as UIElement; }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(UIElement),
            typeof(MetroCircleButton));

        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize",
            typeof(double),
            typeof(MetroCircleButton));

        public new FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight",
            typeof(FontWeight),
            typeof(MetroCircleButton));


        private bool _isAccented;
        public bool IsAccented
        {
            get => this._isAccented;
            set
            {
                if (this._isAccented == value) return;

                if (value)
                {
                    this.SetResourceReference(Control.BackgroundProperty, "AccentColorBrush");
                    this.SetResourceReference(Control.BorderBrushProperty, "AccentColorBrush");
                }
                else
                {
                    this.Background = Brushes.Transparent;
                    this.SetResourceReference(Control.BorderBrushProperty, "ControlBorderBrush");
                }

                this._isAccented = value;
            }
        }

        public MetroCircleButton()
        {
            this.InitializeComponent();

            this._isAccented = false;


            this.FontSize = 18;
            this.FontWeight = FontWeights.Bold;
        }
    }
}
