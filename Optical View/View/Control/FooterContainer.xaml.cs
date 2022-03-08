using System;
using System.Windows;
using System.Windows.Controls;
using static Optical_View.Model.ViewStaticMod;

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
                MainForm.control.WindowState = WindowState.Maximized;
            }
            else
            {
                MainForm.control.WindowState = WindowState.Normal;
            }

            #endregion
        }



    }
}
