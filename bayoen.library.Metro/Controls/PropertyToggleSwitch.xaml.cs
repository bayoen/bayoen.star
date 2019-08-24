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
    public partial class PropertyToggleSwitch : Grid
    {
        public string Path { get; set; }
        public string Header { get; set; }

        private string _detail;
        public string Detail
        {
            get => this._detail;
            set
            {
                if (this._detail == value) return;

                if (value == "")
                {
                    this.HeaderTextBlock.Margin = new Thickness(10, 10, 5, 10);
                    this.DetailTextBlock.Visibility = Visibility.Collapsed;
                    Grid.SetRowSpan(this.ToggleSwitch, 1);
                }
                else
                {
                    this.HeaderTextBlock.Margin = new Thickness(10, 10, 5, 2.5);
                    this.DetailTextBlock.Visibility = Visibility.Visible;
                    Grid.SetRowSpan(this.ToggleSwitch, 2);
                }

                this._detail = value;
            }
        }

        public bool Value { get; set; }
        public string TrueLabel { get; set; }
        public string FalseLabel { get; set; }

        public PropertyToggleSwitch()
        {
            this.InitializeComponent();

            this.Path = "Property";
            this.Header = "Header";
            this.Detail = "";

            this.Value = false;
            this.TrueLabel = "True";
            this.FalseLabel = "False";
        }

        public event RoutedEventHandler Toggled
        {
            add { AddHandler(ToggleSwitchIsCheckedChanged, value); }
            remove { RemoveHandler(ToggleSwitchIsCheckedChanged, value); }
        }

        public static readonly RoutedEvent ToggleSwitchIsCheckedChanged = EventManager.RegisterRoutedEvent(
            "ToggleSwitch_IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PropertyToggleSwitch));

        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(ToggleSwitchIsCheckedChanged));
        }
    }
}
