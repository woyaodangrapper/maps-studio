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
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ = new WindowAccentCompositor(this)
        {
            Color = Color.FromArgb(180, 0, 67, 112),
            IsEnabled = true
        };
    }

    private void ExpandBox_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
        }
        else
        {
            WindowState = WindowState.Maximized;
        }
    }

    private void CloseBox_MouseUp(object sender, MouseButtonEventArgs e)
    {
    }

    private void CloseBox_MouseEnter(object sender, MouseEventArgs e)
    {
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

    private void MinimizeBox_MouseEnter(object sender, MouseEventArgs e)
    {
    }

    private void MinimizeBox_MouseUp(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MinimizeBox_MouseLeave(object sender, MouseEventArgs e)
    {
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