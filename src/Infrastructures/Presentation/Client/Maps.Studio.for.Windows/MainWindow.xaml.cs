using Maps.Studio.Themes;
using Themes.Effects;

namespace Maps.Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            MouseMove += new MouseEventHandler(_MouseMove);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _ = new WindowAccentCompositor(this)
            {
                Color = Color.FromArgb(180, 45, 45, 48),
                IsEnabled = true
            };
        }

        private void _MouseMove(object sender, MouseEventArgs e)
        {
            _ = Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }));
        }

        //关闭按钮，最大化按钮鼠标悬浮事件

        private void CloseBox_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void CloseBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CloseBox_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void ExpandBox_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void ExpandBox_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void Hyperlinks_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void Hyperlinks_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void Hyperlinks_MouseUp(object sender, MouseEventArgs e)
        {
            new StudioWindow().Show();
            Close();
        }

        private void ExpandBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExpandBox_Click();
        }

        private void ExpandBox_Click()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else
            {
                WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(4, 4, 4, 4);
            }
        }

        private void ExpandBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ExpandBox_Click();
        }

        private void _folder_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void _extract_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void _record_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void OBJ_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void GLB_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void FBX_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void GLTF_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Tiles_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}