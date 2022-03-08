using System;
using System.Windows;
using System.Windows.Controls;
using static Optical_View.Model.StaticViewMod;

namespace Optical_View.View.Control
{
    /// <summary>
    /// FooterContainer.xaml 的交互逻辑
    /// </summary>
    public partial class FooterContainer : UserControl
    {
        public FooterContainer()
        {
            InitializeComponent();
        }

        private void openBox_Click(object sender, RoutedEventArgs e)
        {
            #region 全屏拓展
            if ((Boolean)openBox.IsChecked)
            {
                Main.Assembly.WindowState = WindowState.Maximized;
            }
            else
            {
                Main.Assembly.WindowState = WindowState.Normal;
            }

            #endregion
        }



    }
}
