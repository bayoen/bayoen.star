using System;
using System.Collections;
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
    public partial class PropertyComboBox : Grid
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
            typeof(PropertyComboBox), 
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
                    Grid.SetRowSpan(this.ComboBox, 1);
                }
                else
                {
                    this.HeaderTextBlock.Margin = new Thickness(10, 10, 5, 2.5);
                    this.DetailTextBlock.Visibility = Visibility.Visible;
                    Grid.SetRowSpan(this.ComboBox, 2);
                }

                this._detail = value;
            }
        }

        public static readonly DependencyProperty DetailProperty = DependencyProperty.Register(
            "Detail",
            typeof(string),
            typeof(PropertyComboBox),
            new FrameworkPropertyMetadata(string.Empty));

        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(PropertyComboBox),
            new FrameworkPropertyMetadata((int)-1));

        public object SelectedItem => this.ComboBox.SelectedItem;
        public string SelectedString => this.SelectedItem.ToString();

        public IEnumerable ItemSource
        {
            get => this.ComboBox.ItemsSource;
            set
            {
                if (value == null)
                {
                    this.ComboBox.ItemsSource = null;
                    return;
                }
                else if (IEnumerable.Equals(this.ComboBox.ItemsSource, value)) return;

                this.ComboBox.ItemsSource = value;
            }
        }

        public PropertyComboBox()
        {
            this.InitializeComponent();

            this.Property = "Property";
            this.Header = "Header";
            this.Detail = "Detail";
        }

        public event RoutedEventHandler SelectionChanged
        {
            add { this.AddHandler(ComboBoxSelectionChanged, value); }
            remove { this.RemoveHandler(ComboBoxSelectionChanged, value); }
        }
        public double ComboBoxWidth
        {
            get { return (double)this.GetValue(ComboBoxWidthProperty); }
            set { this.SetValue(ComboBoxWidthProperty, value); }
        }
        public static readonly DependencyProperty ComboBoxWidthProperty = DependencyProperty.Register(
            "ComboBoxWidth",
            typeof(double),
            typeof(PropertyComboBox),
            new FrameworkPropertyMetadata((double)100));
       
        public Brush ComboBoxBackground
        {
            get { return this.GetValue(ComboBoxBackgroundProperty) as Brush; }
            set { this.SetValue(ComboBoxBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ComboBoxBackgroundProperty = DependencyProperty.Register(
            "ComboBoxBackground",
            typeof(Brush),
            typeof(PropertyComboBox),
            new FrameworkPropertyMetadata());

        public static readonly RoutedEvent ComboBoxSelectionChanged = EventManager.RegisterRoutedEvent(
            "ComboBox_SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(PropertyComboBox));

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(ComboBoxSelectionChanged));
        }
    }
}
