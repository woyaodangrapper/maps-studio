using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace DeploymentTools
{
    public class LabelEx : Label
    {

        #region Property
        public Direction TextOrientation
        {
            get { return (Direction)GetValue(TextOrientationProperty); }
            set { SetValue(TextOrientationProperty, value); InvalidateVisual(); }
        }
        public static readonly DependencyProperty TextOrientationProperty = DependencyProperty.Register("TextOrientation",
            typeof(Direction), typeof(LabelEx), new PropertyMetadata(Direction.Horizontal));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); InvalidateVisual(); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(LabelEx), new PropertyMetadata(string.Empty));

        public int FontSpace
        {
            get { return (int)GetValue(FontSpaceProperty); }
            set { SetValue(FontSpaceProperty, value); InvalidateVisual(); }
        }
        public static readonly DependencyProperty FontSpaceProperty = DependencyProperty.Register("FontSpace",
            typeof(int), typeof(LabelEx), new PropertyMetadata(0));

        public Brush BackColor
        {
            get { return (Brush)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); InvalidateVisual(); }
        }
        public static readonly DependencyProperty BackColorProperty = DependencyProperty.Register("BackColor",
            typeof(Brush), typeof(LabelEx), new PropertyMetadata(Brushes.Transparent));

        #endregion

        #region override method

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //background
            drawingContext.DrawRectangle(BackColor, null, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));


            Size size = getTextRange();
            double offsetx = 0, offsety = 0;

            if (HorizontalContentAlignment == HorizontalAlignment.Center)
                offsetx = (Width - size.Width - BorderThickness.Left - BorderThickness.Right) / 2;
            if (HorizontalContentAlignment == HorizontalAlignment.Right)
                offsetx = Width - size.Width - BorderThickness.Right;
            if (VerticalContentAlignment == VerticalAlignment.Center)
                offsety = (Height - size.Height - BorderThickness.Top - BorderThickness.Bottom) / 2;
            if (VerticalContentAlignment == VerticalAlignment.Bottom)
                offsety = Height - size.Height - BorderThickness.Bottom;


            drawingContext.PushTransform(new TranslateTransform(offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0));


            //textOrientation
            if (TextOrientation == Direction.Horizontal)
            {
                double w = BorderThickness.Left;
                foreach (char str in Text)
                {
                    var fmttxt = MeasureFont(str.ToString());
                    drawingContext.DrawText(fmttxt, new Point(w, BorderThickness.Top));
                    w += fmttxt.WidthIncludingTrailingWhitespace + FontSpace;
                }
            }
            else if (TextOrientation == Direction.Vertical)
            {
                drawingContext.PushTransform(new TranslateTransform(-Math.Abs(size.Width - size.Height) / 2, Math.Abs(size.Width - size.Height) / 2));

                double w = BorderThickness.Top;
                drawingContext.PushTransform(new RotateTransform(90, size.Height / 2, size.Width / 2 + BorderThickness.Top / 2));
                foreach (char str in Text)
                {
                    var fmttxt = MeasureFont(str.ToString());
                    if (IsASCIIChar(str))
                    {
                        drawingContext.DrawText(fmttxt, new Point(w, BorderThickness.Top));
                        w += fmttxt.WidthIncludingTrailingWhitespace + FontSpace;
                    }
                    else
                    {
                        drawingContext.PushTransform(new TranslateTransform(-2, 2.5));
                        drawingContext.PushTransform(new RotateTransform(-90, w + fmttxt.WidthIncludingTrailingWhitespace / 2, fmttxt.Height / 2));
                        drawingContext.DrawText(fmttxt, new Point(w, BorderThickness.Top));
                        drawingContext.Pop();
                        drawingContext.Pop();
                        w += fmttxt.WidthIncludingTrailingWhitespace / 2 + fmttxt.Height / 2 + FontSpace;
                    }
                }
                drawingContext.Pop();
                drawingContext.Pop();
            }
            else if (TextOrientation == Direction.ClockwiseVertical)
            {
                drawingContext.PushTransform(new TranslateTransform(-Math.Abs(size.Width - size.Height) / 2, Math.Abs(size.Width - size.Height) / 2));
                double w = BorderThickness.Top;
                drawingContext.PushTransform(new RotateTransform(90, size.Height / 2, size.Width / 2 + BorderThickness.Top / 2));
                foreach (char str in Text)
                {
                    var fmttxt = MeasureFont(str.ToString());
                    drawingContext.DrawText(fmttxt, new Point(w, BorderThickness.Top));
                    w += fmttxt.WidthIncludingTrailingWhitespace + FontSpace;
                }
                drawingContext.Pop();
                drawingContext.Pop();
            }
            else
            {
                drawingContext.PushTransform(new TranslateTransform(-Math.Abs(size.Width - size.Height) / 2, Math.Abs(size.Width - size.Height) / 2));
                double w = BorderThickness.Top;
                drawingContext.PushTransform(new RotateTransform(270, size.Height / 2, size.Width / 2 + BorderThickness.Top / 2));
                foreach (char str in Text)
                {
                    var fmttxt = MeasureFont(str.ToString());
                    drawingContext.DrawText(fmttxt, new Point(w, BorderThickness.Top));
                    w += fmttxt.WidthIncludingTrailingWhitespace + FontSpace;
                }
                drawingContext.Pop();
                drawingContext.Pop();
            }

            drawingContext.Pop();

            if (double.IsNaN(Height))
                Height = size.Height + BorderThickness.Top + BorderThickness.Bottom;
            if (double.IsNaN(Width))
                Width = size.Width + BorderThickness.Left + BorderThickness.Right;
        }

        #endregion

        #region other method 

        private bool IsASCIIChar(char c)
        {
            // 判断字符串中第一位字符是否是ASCII字符（ 0–127）,ASCII字符占一个字节	
            return c / 0x80 == 0 ? true : false;
        }

        [Obsolete]
        private FormattedText MeasureFont(string str)
        {
            var ftxt = new FormattedText(
                        str,
                        CultureInfo.CurrentUICulture,
                        FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                        FontSize, Foreground

                        );
            ftxt.TextAlignment = TextAlignment.Justify;
            return ftxt;
        }

        /// <summary>
        /// get all Text area size
        /// </summary>
        /// <returns></returns>
        private Size getTextRange()
        {
            Size size = new Size();
            if (TextOrientation == Direction.Horizontal)
                foreach (var v in Text)
                {
                    var ft = MeasureFont(v.ToString());
                    size.Width += ft.WidthIncludingTrailingWhitespace + FontSpace;
                    size.Height = size.Height > ft.Height ? size.Height : ft.Height;
                }
            else if (TextOrientation == Direction.Vertical)
            {
                foreach (var v in Text)
                {
                    var ft = MeasureFont(v.ToString());
                    if (IsASCIIChar(v))
                        size.Height += ft.Width + FontSpace;
                    else
                        size.Height += ft.Height / 2 + ft.WidthIncludingTrailingWhitespace / 2 + FontSpace;
                    size.Width = size.Width > ft.Height ? size.Width : ft.Height;
                }
            }
            else
            {
                foreach (var v in Text)
                {
                    var ft = MeasureFont(v.ToString());
                    size.Height += ft.WidthIncludingTrailingWhitespace + FontSpace;
                    size.Width = size.Width > ft.Height ? size.Width : ft.Height;
                }
            }
            return size;
        }
        #endregion
        public enum Direction
        {
            Horizontal = 0,
            Vertical = 1,
            ClockwiseVertical = 1,
            AntiClockwiseVertical = 3
        }

    }
}
