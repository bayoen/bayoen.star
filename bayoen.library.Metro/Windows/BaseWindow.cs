using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using MahApps.Metro.Controls;

using bayoen.library.General.Enums;

namespace bayoen.library.Metro.Windows
{
    /// <summary>
    /// Base window for project bayoen from '<see cref="MetroWindow"/>'
    /// </summary>
    public class BaseWindow : MetroWindow
    {
        public BaseWindow()
        {
            this.TitleCharacterCasing = CharacterCasing.Normal;
            this.ShowIconOnTitleBar = false;
            this.MinHeight = this.MinWidth = 250;
            this.SetResourceReference(Control.BackgroundProperty, "WindowBackgroundBrush");
            this.BorderThickness = new Thickness(1);
            this.WindowTitleBrush = Brushes.Transparent;

            this.MouseLeftButtonDown += BaseWindow_MouseLeftButtonDown;
            this.Closing += BaseWindow_Closing;

            this.CloseToMinimize = false;
            this.CloseToKill = false;
            this.IsWindowDraggable = true;
            this.IsFixed = false;
        }

        /// <summary>
        /// When closed, if it is <see cref="true"/>, <see cref="BaseWindow"/> is minimized
        /// </summary>
        public bool CloseToMinimize
        {
            get => (bool)GetValue(CloseToMinimizeProperty);
            set => SetValue(CloseToMinimizeProperty, value);
        }
        public static readonly DependencyProperty CloseToMinimizeProperty = DependencyProperty.Register(
            "CloseToMinimize",
            typeof(bool),
            typeof(BaseWindow));

        /// <summary>
        /// When closed, if it is <see cref="true"/>, <see cref="BaseWindow"/> is terminated
        /// </summary>
        public bool CloseToKill
        {
            get => (bool)GetValue(CloseToKillProperty);
            set => SetValue(CloseToKillProperty, value);
        }
        public static readonly DependencyProperty CloseToKillProperty = DependencyProperty.Register(
            "CloseToKill",
            typeof(bool),
            typeof(BaseWindow));

        /// <summary>
        /// If <see cref="BaseWindow"/> is fixed, <see cref="Window.DragMove"/> is disabled
        /// </summary>
        public bool IsFixed
        {
            get => (bool)GetValue(IsFixedProperty);
            set => SetValue(IsFixedProperty, value);
        }
        public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register(
            "IsFixed",
            typeof(bool),
            typeof(BaseWindow));

        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.CloseToKill)
            {

            }
            else
            {
                e.Cancel = true;
                if (this.CloseToMinimize)
                {
                    this.WindowState = WindowState.Minimized;
                }
                else
                {
                    this.Hide();
                }
            }
        }

        private void BaseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsFixed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Show and activate '<see cref="BaseWindow"/>'
        /// </summary>
        public new void Show()
        {
            base.Show();
            this.Activate();
        }
    }
}
