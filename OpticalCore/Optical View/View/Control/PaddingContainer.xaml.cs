using Optical_View.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// HeaderContainer.xaml 的交互逻辑
    /// </summary>
    public partial class PaddingContainer : UserControl
    {
        /// <summary>
        /// 当前用户控件继承看manwin的WS  （=-=）
        /// </summary>
        public static Window @this { get; private set; }
        public PaddingContainer()
        {
            InitializeComponent();
            @this = MainForm.control;

            #region 地球模型
            TopEarth3D.Visibility = Visibility.Visible;
            bool moment = true;//线程开关
            _ = Task.Factory.StartNew(() =>
              {
                  while (moment)
                  {
                      Thread.Sleep(10);
                      Dispatcher.BeginInvoke(new Action(delegate
                      {
                        #region 球体旋转
                        if (TopEarth3D.Rotation3D.Angle >= 360)
                          {
                              TopEarth3D.Rotation3D.Angle = 0;
                              moment = false;
                          }
                          TopEarth3D.Rotation3D.Angle++;
                        #endregion

                    }));
                  }
              });
            #endregion
            #region 顶部栏设置
            ///鼠标移动
            TopBder.MouseLeftButtonDown += Grid_MouseDown;
            TopBder.MouseMove += new MouseEventHandler(TopBder_MouseMove);
            void TopBder_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //处理win10 边框磁吸效果
                    if (@this != null && @this.WindowState == WindowState.Maximized)
                    {
                        // Gets the absolute mouse position, relative to screen
                        System.Windows.Point GetMousePos()
                        {
                            return PointToScreen(Mouse.GetPosition(this));
                        }
                        System.Windows.Point T = GetMousePos();
                        @this.Top = T.Y - (Mouse.GetPosition(this).Y);
                        @this.Left = @this.Left / 2;
                        @this.WindowState = WindowState.Normal;

                    }
                    if (@this != null) @this.DragMove();
                }
            }
            void Grid_MouseDown(object sender, MouseButtonEventArgs e)
            {

                //双击
                switch (e.ClickCount)
                {
                    case 1://单击
                        {

                            break;
                        }
                    case 2://双击
                        {
                            if (@this.WindowState == WindowState.Maximized)
                            {
                                @this.WindowState = WindowState.Normal;

                            }
                            else
                            {
                                @this.WindowState = WindowState.Maximized;
                            }


                            break;
                        }
                }
            }
            #endregion
        
        }


        private void ICON_MouseUp(object sender, MouseButtonEventArgs e)
        {


        }
        //大标题点击事件
        private void MainTitle_MouseUp(object sender, MouseButtonEventArgs e)
        {

            #region 打开GITHUB网页
            string url = "https://github.com/light-come/Cesium-OpticalCore";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //不使用shell启动
            p.StartInfo.RedirectStandardInput = true;//喊cmd接受标准输入
            p.StartInfo.RedirectStandardOutput = false;//不想听cmd讲话所以不要他输出
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示窗口
            p.Start();

            //向cmd窗口发送输入信息 后面的&exit告诉cmd运行好之后就退出
            p.StandardInput.WriteLine("start " + url + "&exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            #endregion
        }

        private void TitleLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_Browser.webView != null && _Browser.webView.CoreWebView2 != null)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                _Browser.webView.Source = new Uri("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString() + "/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
            }
        }
    }
}
