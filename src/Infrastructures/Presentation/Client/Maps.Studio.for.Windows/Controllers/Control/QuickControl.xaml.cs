using MaterialDesignThemes.Wpf;

namespace Maps.Studio.Themes.Controls;

/// <summary>
/// QuickControl.xaml 的交互逻辑
/// </summary>
public partial class QuickControl : UserControl
{
    public QuickControl()
    {
        InitializeComponent();
        var controls = new object[6] { ExpandBord, CloseBord, MiniBord, ExpandIcon, CloseIcon, MiniIcon };
        foreach (var box in controls)
        {
            switch (box)
            {
                case Border border:
                    border.MouseEnter += (sender, e) =>
                    {
                        if (border.Name != "CloseBord")
                            border.Background = new SolidColorBrush(Color.FromArgb(255, 63, 63, 63));
                        else
                            border.Background = new SolidColorBrush(Color.FromArgb(180, 224, 100, 100));
                    };
                    border.MouseLeave += (sender, e) => { border.Background = new SolidColorBrush(Color.FromArgb(255, 45, 45, 48)); };
                    break;

                case PackIcon packIcon:
                    packIcon.MouseEnter += (sender, e) =>
                    {
                        var border = controls.FirstOrDefault(x => x is Border s && s.Name == $"{packIcon.Name[0..^4]}Bord") as Border;
                        if (border is not null)
                            if (border.Name != "CloseBord")
                                border.Background = new SolidColorBrush(Color.FromArgb(255, 63, 63, 63));
                            else
                                border.Background = new SolidColorBrush(Color.FromArgb(180, 224, 100, 100));
                    };
                    packIcon.MouseLeave += (sender, e) => { };
                    break;
            }
        }
    }
}