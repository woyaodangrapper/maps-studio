using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_View.Class;
using Optical_View.Model;
using System;
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
        static Appsettings appsettings = new Appsettings();


        public Launch_Window()
        {
            InitializeComponent();
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
                TextBlock textBlock_path = new TextBlock() {
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
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(200, 0, 122, 204));
        }

        private void Hyperlinks_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HyperlinksLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void Hyperlinks_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            new MainWindow().Show();
            Launch.Startupz_type.Type = "Hyperlinks";
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

        private void _folder_MouseUp(object sender, MouseButtonEventArgs e)
        {

            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var file = dialog.FileName;

                Launch.Startupz_type.Path = file;
                Launch.Startupz_type.Type = "_folder";

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

                Launch.Startupz_type.Path = file;
                Launch.Startupz_type.Type = "_extract";

                new MainWindow().Show();
                Close();
            }

        }

        private void _record_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
           
            var path = ((TextBlock)grid.Children[1]).Text;
            var type = ((TextBlock)grid.Children[2]).Text.Replace(".","").ToLower();

            Launch.Startupz_type.Path = path;
            Launch.Startupz_type.Type = type;

            new MainWindow().Show();
            Close();

        }

        /// <summary>
        /// 选择文件
        /// </summary>
        string SelectFile(String Title,String type)
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
            string path = SelectFile("请打开一个或多个OBJ","obj");
            if (path != null) {
                Launch.Startupz_type.Path = path;
                Launch.Startupz_type.Type = "obj";

                new MainWindow().Show();
                Close();
            }
        }

        private void GLB_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "glb");
            if (path != null)
            {
                Launch.Startupz_type.Path = path;
                Launch.Startupz_type.Type = "glb";

                new MainWindow().Show();
                Close();
            }
        }

        private void FBX_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "fbx");
            if (path != null)
            {
                Launch.Startupz_type.Path = path;
                Launch.Startupz_type.Type = "fbx";

                new MainWindow().Show();
                Close();
            }
        }

        private void GLTF_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "gltf");
            if (path != null)
            {
                Launch.Startupz_type.Path = path;
                Launch.Startupz_type.Type = "gltf";

                new MainWindow().Show();
                Close();
            }
        }

        private void Tiles_BUT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = SelectFile("请选择文件", "3d-tiles");
            if (path != null)
            {
                Launch.Startupz_type.Path = path;
                Launch.Startupz_type.Type = "3dtiles";

                new MainWindow().Show();
                Close();
            }

        }

       

    }
}
