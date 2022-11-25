using Themes.Effects;

namespace Maps.Studio.Themes;

/// <summary>
/// StudioWindow.xaml 的交互逻辑
/// </summary>
public partial class MapsWindow : Window
{
    public MapsWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ = new WindowAccentCompositor(this)
        {
            Color = Color.FromArgb(0, 0, 67, 112),
            IsEnabled = true
        };
    }
}