using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace bayoen.star.Controls
{
    public partial class WindowCommandButton : Button
    {
        public Thickness AppbarMargin { get; set; }

        private string _appbarString;
        public string AppbarString
        {
            get => this._appbarString;
            set
            {
                if (this._appbarString == value) return;

                this.AppbarBrush = TryFindResource(value) as Visual;
                this._appbarString = value;
            }
        }
        public Visual AppbarBrush { get; private set; }
        public double AppbarHeight { get; set; }
        public double AppbarWidth { get; set; }

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(WindowCommandButton));

        public WindowCommandButton()
        {
            this.InitializeComponent();

            this.AppbarHeight = 15;
            this.AppbarWidth = 13;
            this.AppbarMargin = new Thickness(1, 0, 3, 0);
            this.Padding = new Thickness(4, 0, 6, 0);
        }
    }
}
