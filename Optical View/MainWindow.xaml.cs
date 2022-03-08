using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static Optical_View.Model.StaticViewMod;

namespace Optical_View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {


            #region 初始化用户控件 扔入公共控件类
            Window w = this;
            Main.Assembly = w;
            #endregion
            InitializeComponent();

            #region 初始化用户控件 扔入公共控件类

            #endregion

        }


        #region 关闭按钮，最大化按钮鼠标悬浮事件
        //关闭按钮，最大化按钮鼠标悬浮事件
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
            ClearDirectory(@"WebGL\.cache");
            System.Environment.Exit(0);
        }
        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        /// <remarks>删除指定文件夹中所有文件</remarks>
        public static bool ClearDirectory(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)
                    || !Directory.Exists(path))
                {
                    return true;  // 如果参数为空，则视为已成功清空
                }
                // 删除当前文件夹下所有文件
                foreach (string strFile in Directory.GetFiles(path))
                {
                    File.Delete(strFile);
                }
                // 删除当前文件夹下所有子文件夹(递归)
                foreach (string strDir in Directory.GetDirectories(path))
                {
                    Directory.Delete(strDir, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("清空 {0} 异常, 消息:{1}, 堆栈:{2}"
                    , path, ex.Message, ex.StackTrace));
                return false;
            }
        }

        private void CloseBox_MouseEnter(object sender, MouseEventArgs e)
        {
            WindowCloseBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(180, 224, 100, 100));
        }
        private void CloseBox_MouseLeave(object sender, MouseEventArgs e)
        {
            WindowCloseBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 45, 45, 48));
            //Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }


        private void ExpandBox_MouseEnter(object sender, MouseEventArgs e)
        {
            CircleExpandBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 63, 63));

            //FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
        }
        private void ExpandBox_MouseLeave(object sender, MouseEventArgs e)
        {
            CircleExpandBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 45, 45, 48));
            //Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }

        private void MinimizeBox_MouseEnter(object sender, MouseEventArgs e)
        {
            MinimizeBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 63, 63));
        }
        private void MinimizeBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MinimizeBox_MouseLeave(object sender, MouseEventArgs e)
        {
            MinimizeBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 45, 45, 48));
            //Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }


        #endregion
        #region 首页快捷键
        //窗体热键
        private void F12_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            switch (((System.Windows.Input.RoutedUICommand)e.Command).Text)
            {
                case "F12":


                    break;
                default:
                    return;
            }
        }
        #endregion

    }
}
