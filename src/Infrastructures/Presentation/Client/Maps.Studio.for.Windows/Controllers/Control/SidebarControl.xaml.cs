namespace Maps.Studio.Themes.Controls
{
    /// <summary>
    /// Sidebar.xaml 的交互逻辑
    /// </summary>
    public partial class SidebarContainer : UserControl
    {
        public SidebarContainer()
        {
            InitializeComponent();
            SideUnfoldingGrid.Opacity = 0;
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ENDBox.Background = new SolidColorBrush(Color.FromArgb(255, 20, 134, 211));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void BorderT1_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void BorderT1_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void Monitor_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void ENDBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void FileTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }
    }
}