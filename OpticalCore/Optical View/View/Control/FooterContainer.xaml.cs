using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
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
        private void BackgroundMongolia_Style(int type)
        {
            switch (type)
            {
                case 0:
                    HwndSource hwndSource = PresentationSource.FromVisual(MainForm.control) as HwndSource;
                    if (hwndSource != null)
                    {
                        var handle = hwndSource.Handle;
                        new Class.GroundGlass().EnableBlur(handle);
                    }
                    break;
                default:
                    break;
            }


        }


    }
}
