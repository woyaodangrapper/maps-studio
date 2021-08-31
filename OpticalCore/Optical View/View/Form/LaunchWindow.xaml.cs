using Optical_View.Class;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Optical_View.View.Form
{
    /// <summary>
    /// Launch_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Launch_Window : Window
    {
        public Launch_Window()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(this.Window_Loaded);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                var handle = hwndSource.Handle;
                new GroundGlass().EnableBlur(handle);
            }
            MouseMove += new System.Windows.Input.MouseEventHandler(_MouseMove);

        }
        void _MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _=Dispatcher.BeginInvoke(new Action(() => {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }

            }));
        }
        #region 关闭按钮，最大化按钮鼠标悬浮事件
        //关闭按钮，最大化按钮鼠标悬浮事件

        private void CloseBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            WindowCloseBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(180, 224, 100, 100));
        }
      
        private void CloseBox_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Application.Current.Shutdown();
            System.Environment.Exit(0);
        }
        private void CloseBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            WindowCloseBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 45, 45, 48));
            //Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }
        private void ExpandBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CircleExpandBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 63, 63));

            //FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
        }

        private void ExpandBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CircleExpandBord.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 45, 45, 48));
            //Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }

        private void Hyperlinks_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(100, 0, 122, 204));
        }

        private void Hyperlinks_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
        }

        private void Hyperlinks_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
        
        #endregion
        #region 最大化按钮单击事件
        private void ExpandBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExpandBox_Click();
        }
        private void ExpandBox_Click()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else
            {
                WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(4, 4, 4, 4);
            }

        }
        private void ExpandBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ExpandBox_Click();
        }

        #endregion
       
    }
}
