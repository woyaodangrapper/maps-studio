using Themes.Effects;

namespace Maps.Studio.Themes;

public partial class StudioWindow : Window
{
    public StudioWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        ContentRendered += OnRendered;
    }

    private void OnRendered(object? s, EventArgs e)
    {
        //var bords = FindVisualChildren<Border>(QuickControl.QuickGrid, "Bord").ToArray();
        //var icons = FindVisualChildren<PackIcon>(QuickControl.QuickGrid, "Icon").ToArray();
        //var arr = new List<object>(bords).Concat(icons).ToList();

        var bps = VisualExtension.FindVisualChildren<Border, PackIcon>(QuickControl.QuickGrid).ToList()
            .FindAll(x => (x as FrameworkElement)?.Name is not null and not "")
            .Select(x => x as FrameworkElement).ToList();

        foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Close")))
        {
            item!.MouseUp += (s, e) => Close();
        }

        foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Mini")))
        {
            item!.MouseUp += (s, e) => WindowState = WindowState.Minimized;
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

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ = new WindowAccentCompositor(this)
        {
            Color = Color.FromArgb(180, 0, 67, 112),
            IsEnabled = true
        };
    }

    //窗体热键
    private void F12_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        switch (((RoutedUICommand)e.Command).Text)
        {
            case "F12":

                break;

            default:
                return;
        }
    }
}