using System;
using System.Windows;
using System.Windows.Controls;

namespace bayoen.library.Metro.Controls
{
    public partial class PropertyNumericUpDown : Grid
    {
        public PropertyNumericUpDown()
        {
            this.InitializeComponent();

            this.Property = "Property";
            this.Header = "Header";
            this.Detail = "Detail";

            this.Value = 0;       
        }

        public string Property { get; set; }
        public string Header
        {
            get { return this.GetValue(HeaderProperty) as string; }
            set { this.SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(PropertyNumericUpDown),
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
                    Grid.SetRowSpan(this.NumericUpDown, 1);
                }
                else
                {
                    this.HeaderTextBlock.Margin = new Thickness(10, 10, 5, 2.5);
                    this.DetailTextBlock.Visibility = Visibility.Visible;
                    Grid.SetRowSpan(this.NumericUpDown, 2);
                }

                this._detail = value;
            }
        }
        public static readonly DependencyProperty DetailProperty = DependencyProperty.Register(
            "Detail",
            typeof(string),
            typeof(PropertyNumericUpDown),
            new FrameworkPropertyMetadata(string.Empty));


        public int Value
        {
            get { return (int)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(int),
            typeof(PropertyNumericUpDown),
            new FrameworkPropertyMetadata(0));
        
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(NumericUpDownValueChanged, value); }
            remove { RemoveHandler(NumericUpDownValueChanged, value); }
        }
        public static readonly RoutedEvent NumericUpDownValueChanged = EventManager.RegisterRoutedEvent(
            "NumericUpDown_ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PropertyNumericUpDown));
        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            this.RaiseEvent(new RoutedEventArgs(NumericUpDownValueChanged));
        }
    }
}




