using MaterialDesignThemes.Wpf;
using Themes.Effects;

namespace Maps.Studio.Themes;

/// <summary>
/// MapsWindow.xaml 的交互逻辑
/// </summary>
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

        var bps = FindVisualChildren<Border, PackIcon>(QuickControl.QuickGrid).ToList()
            .FindAll(x => (x as FrameworkElement)?.Name is not null and not "")
            .Select(x => x as FrameworkElement).ToList();

        foreach (var item in bps.FindAll(x => x is not null && x.Name.Contains("Close")))
        {
            item!.MouseUp += (s, e) => Close();
        }

        foreach (var item in bps.FindAll(x => x!.Name.Contains("Mini")))
        {
            item!.MouseUp += (s, e) => WindowState = WindowState.Minimized;
        }

        foreach (var item in bps.FindAll(x => x!.Name.Contains("Expand")))
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

    public IEnumerable<T> FindVisualChildren<T>(DependencyObject? obj, string name = "") where T : FrameworkElement
    {
        if (obj is not null)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is not null and T && ((T)child).Name.Contains(name))
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child, name))
                    yield return childOfChild;
            }
    }

    public IEnumerable<object> FindVisualChildren<T1, T2>(DependencyObject? obj)
       where T1 : FrameworkElement
       where T2 : FrameworkElement
    {
        if (obj is not null)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is not null and T1 or T2)
                    if (child is T1 a)
                        yield return a;
                    else
                    if (child is T2 b)
                        yield return b;

                foreach (object childOfChild in FindVisualChildren<T1, T2>(child))
                {
                    yield return childOfChild;
                }
            }
    }

    public static T? GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);

        while (parent != null)
        {
            if (parent is T t && (t.Name == name))
                return t;
            else
                parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
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