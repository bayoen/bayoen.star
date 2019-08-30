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
        public string Property { get; set; }
        public string Header
        {
            get { return this.GetValue(HeaderProperty) as string; }
            set { this.SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(PropertyToggleSwitch),
            new FrameworkPropertyMetadata(string.Empty));

        private string _detail
        {
            get { return this.GetValue(DetailProperty) as string; }
            set { this.SetValue(DetailProperty, value); }
        }
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
        public static readonly DependencyProperty DetailProperty = DependencyProperty.Register(
            "Detail",
            typeof(string),
            typeof(PropertyToggleSwitch),
            new FrameworkPropertyMetadata(string.Empty));


        public bool Value
        {
            get { return (bool)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(bool),
            typeof(PropertyToggleSwitch),
            new FrameworkPropertyMetadata(false));

        public string TrueLabel
        {
            get { return this.GetValue(TrueLabelProperty) as string; }
            set { this.SetValue(TrueLabelProperty, value); }
        }
        public static readonly DependencyProperty TrueLabelProperty = DependencyProperty.Register(
            "TrueLabel",
            typeof(string),
            typeof(PropertyToggleSwitch),
            new FrameworkPropertyMetadata(string.Empty));

        public string FalseLabel
        {
            get { return this.GetValue(FalseLabelProperty) as string; }
            set { this.SetValue(FalseLabelProperty, value); }
        }
        public static readonly DependencyProperty FalseLabelProperty = DependencyProperty.Register(
            "FalseLabel",
            typeof(string),
            typeof(PropertyToggleSwitch),
            new FrameworkPropertyMetadata(string.Empty));

        public PropertyToggleSwitch()
        {
            this.InitializeComponent();

            this.Property = "Property";
            this.Header = "Header";
            this.Detail = "Detail";

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
