using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Optical_View.Model.View_static_control;

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
