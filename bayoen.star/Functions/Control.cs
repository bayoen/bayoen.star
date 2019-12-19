using System.Windows;

namespace bayoen.star.Functions
{
    public class Control
    {
        public static void MakeDraggableControl(FrameworkElement control, bool controlFlag)
        {
            Point anchorPoint = new Point();
            Point currentPoint = new Point();
            bool isInDrag = false;

            control.VerticalAlignment = VerticalAlignment.Top;
            control.HorizontalAlignment = HorizontalAlignment.Left;

            control.MouseLeftButtonDown += (sender, e) =>
            {
                anchorPoint = e.GetPosition(null);

                (sender as FrameworkElement).CaptureMouse();

                if (controlFlag) isInDrag = true;
                e.Handled = true;
            };

            control.MouseMove += (sender, e) =>
            {
                if (isInDrag)
                {
                    currentPoint = e.GetPosition(null);

                    (sender as FrameworkElement).Margin = new Thickness(
                        (sender as FrameworkElement).Margin.Left + currentPoint.X - anchorPoint.X,
                        (sender as FrameworkElement).Margin.Top + currentPoint.Y - anchorPoint.Y,
                        (sender as FrameworkElement).Margin.Right,
                        (sender as FrameworkElement).Margin.Bottom);

                    anchorPoint = currentPoint;
                }
            };

            control.MouseLeftButtonUp += (sender, e) =>
            {
                if (isInDrag)
                {
                    (sender as FrameworkElement).ReleaseMouseCapture();

                    isInDrag = false;
                    e.Handled = true;
                }
            };
        }
    }
}
