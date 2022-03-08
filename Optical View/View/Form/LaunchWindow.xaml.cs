using Microsoft.Web.WebView2.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_View.Class;
using Optical_View.Model;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Optical_View.View.Form
{
    /// <summary>
    /// Launch_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Launch_Window : Window
    {
        static AppsettingsCS appsettings = new AppsettingsCS();


        public Launch_Window()
        {
            InitializeComponent();
            if (!CheckWebView())
            {
                MessageBox.Show("检测到当前未安装运行环境，正在启动安装程序，请稍后...");
                InstallWebView();
                Stopwatch.StartNew();
                while (!CheckWebView())
                {
                    System.Threading.Thread.Sleep(3000);
                }
            }

            ListView.Items.Clear();
            foreach (var item in appsettings.GetKeys("record"))
            {
                Grid grid = new Grid();
                grid.Height = 403;
                grid.Height = 29;


                TextBlock textBlock_name = new TextBlock()
                {
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Style = (Style)this.FindResource("textBlockStyle"),
                    Text = Path.GetFileNameWithoutExtension(item["path"].ToString()),
                    Width = 296,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(86, 0, 0, 0)
                };
                TextBlock textBlock_path = new TextBlock()
                {
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Style = (Style)this.FindResource("textBlockStyle"),
                    Text = item["path"].ToString(),
                    Width = 280,
                    Height = 11,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(86, 0, 21, 1),
                    FontSize = 8
                };
                TextBlock textBlock_type = new TextBlock()
                {
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Style = (Style)this.FindResource("textBlockStyle"),
                    Text = "." + item["type"].ToString().ToUpper(),
                    Width = 86,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
                };
                grid.ToolTip = item["path"].ToString();
                grid.Children.Add(textBlock_name);
                grid.Children.Add(textBlock_path);
                grid.Children.Add(textBlock_type);
                grid.MouseUp += _record_MouseUp;
                ListView.Items.Add(grid);
            }
            Loaded += new RoutedEventHandler(this.Window_Loaded);
        }

        /// <summary>
        /// 检测是否安装了符合条件的web浏览器
        /// </summary>
        /// <returns></returns>
        private static bool CheckWebView()
        {
            try
            {
                string str = CoreWebView2Environment.GetAvailableBrowserVersionString();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 运行MicrosoftEdgeWebview2Setup安装三维环境
        /// </summary>
        private void InstallWebView()
        {
            string path = Environment.CurrentDirectory + @"\check\MicrosoftEdgeWebview2Setup.exe";
            if (File.Exists(path))
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = Environment.CurrentDirectory + @"\check\";
                p.Start();
                p.StandardInput.WriteLine("MicrosoftEdgeWebview2Setup.exe");
                p.StandardInput.WriteLine("exit");
            }
            else
            {
                //Log.Debug($"Launched from {Environment.CurrentDirectory}");
                //Log.Debug($"Physical location {AppDomain.CurrentDomain.BaseDirectory}");
                //Log.Debug($"AppContext.BaseDir {AppContext.BaseDirectory}");
                //Log.Debug($"Runtime Call {Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}");
                //调用系统默认的浏览器
                MessageBox.Show("非常抱歉,程序关键缺少程序。接下来会打开微软官方。请根据网站提示安装[常青版引导程序]");
                string url = "https://developer.microsoft.com/zh-cn/microsoft-edge/webview2/#download-section";
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


                System.Environment.Exit(0);
            }

        }

        #region 鼠标控件 关闭 最大化 最小化 移动 处理
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                var handle = hwndSource.Handle;
                new GroundGlass().EnableBlur(handle);
            }
            MouseMove += new MouseEventHandler(_MouseMove);

        }
        void _MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _ = Dispatcher.BeginInvoke(new Action(() =>
            {
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
            System.Environment.Exit(0);
            //Application.Current.Shutdown();
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
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(200, 0, 122, 204));
        }

        private void Hyperlinks_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void Hyperlinks_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ProjectMod.HistoricalProject.Type = "Hyperlinks";
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


        #endregion

        ///////////////////按钮点击以及拖动展示时处理//////////////////////////////////////////////////////////

        private void _folder_MouseUp(object sender, MouseButtonEventArgs e)
        {

            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var file = dialog.FileName;
                appsettings.SetKeys("record", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    path = file,
                    type = "_folder"
                })));
                ProjectMod.HistoricalProject.Path = file;
                ProjectMod.HistoricalProject.Type = "_folder";

                new MainWindow().Show();
                Close();
            }

        }

        private void _extract_MouseUp(object sender, MouseButtonEventArgs e)
        {

            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var file = dialog.FileName;
                appsettings.SetKeys("record", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    path = file,
                    type = "_extract"
                })));
                ProjectMod.HistoricalProject.Path = file;
                ProjectMod.HistoricalProject.Type = "_extract";


                new MainWindow().Show();
                Close();
            }

        }

        private void _record_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;

            var path = ((TextBlock)grid.Children[1]).Text;
            var type = ((TextBlock)grid.Children[2]).Text.Replace(".", "").ToLower();

            if (Directory.Exists(path))
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = type;
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("目录不存在");
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        string SelectFile(String Title, String type)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog(Title);

            switch (type)
            {
                case "obj":
                    dialog.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.obj"));
                    break;
                case "3d-tiles":
                    dialog.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.json"));
                    break;
                case "gltf":
                    dialog.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.gltf"));
                    break;
                case "fbx":
                    dialog.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.fbx"));
                    break;
                case "glb":
                    dialog.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.glb"));
                    break;
            }

            dialog.AllowNonFileSystemItems = true;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;

                appsettings.SetKeys("record", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    path = path,
                    type = type
                })));

                return path;
            }
            return null;
        }
        private void OBJ_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请打开一个或多个OBJ", "obj");
            if (path != null)
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = "obj";

                new MainWindow().Show();
                Close();
            }
        }

        private void GLB_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "glb");
            if (path != null)
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = "glb";

                new MainWindow().Show();
                Close();
            }
        }

        private void FBX_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "fbx");
            if (path != null)
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = "fbx";

                new MainWindow().Show();
                Close();
            }
        }

        private void GLTF_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "gltf");
            if (path != null)
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = "gltf";

                new MainWindow().Show();
                Close();
            }
        }

        private void Tiles_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "3d-tiles");
            if (path != null)
            {
                ProjectMod.HistoricalProject.Path = path;
                ProjectMod.HistoricalProject.Type = "3dtiles";

                new MainWindow().Show();
                Close();
            }

        }



    }
}
