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
            ContentRendered += OnRendered;
            MouseMove += new MouseEventHandler(OnMouseMove);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _ = new WindowAccentCompositor(this)
            {
                Color = Color.FromArgb(180, 45, 45, 48),
                IsEnabled = true
            };
        }

        private void OnRendered(object? s, EventArgs e)
        {
            var bps = VisualExtension.FindVisualChildren<Border, PackIcon>(QuickControl.QuickGrid).ToList()
                .FindAll(x => (x as FrameworkElement)?.Name is not null and not "")
                .Select(x => x as FrameworkElement).ToList();

            foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Close")))
            {
                item!.MouseUp += (s, e) => Close();
            }

            foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Mini")))
            {
                item!.Visibility = Visibility.Hidden;
            }

            foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Expand")))
            {
                item!.MouseUp += (s, e) =>
                {
                    if (WindowState == WindowState.Maximized)
                        WindowState = WindowState.Normal;
                    else
                        WindowState = WindowState.Maximized;
                };
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _ = Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }));
        }

        private void Hyperlinks_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(200, 0, 122, 204));
        }

        private void Hyperlinks_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void Hyperlinks_MouseUp(object sender, MouseEventArgs e)
        {
            new StudioWindow().Show();
            Close();
        }
    }
}