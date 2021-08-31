using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Optical_View.Model.View_static_control;

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
                MainForm.control = w;
            #endregion
            InitializeComponent();

            #region 初始化用户控件 扔入公共控件类
                BrowserContainer.control = PaddingContainer._Browser;
            #endregion

            Min.MouseMove += new MouseEventHandler(TopBder_MouseMove);
            void TopBder_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //处理win10 边框磁吸效果
                    if (this != null && WindowState == WindowState.Maximized)
                    {
                        // Gets the absolute mouse position, relative to screen
                        Point GetMousePos()
                        {
                            return PointToScreen(Mouse.GetPosition(this));
                        }
                        Point T = GetMousePos();
                        Top = T.Y - Mouse.GetPosition(this).Y;
                        Left /= 2;
                        WindowState = WindowState.Normal;

                    }
                    if (this != null)
                    {
                        DragMove();
                    }
                }
            }
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
            System.Environment.Exit(0);
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
