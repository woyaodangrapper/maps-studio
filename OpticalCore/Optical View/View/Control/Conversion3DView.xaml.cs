using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using Optical_View.Class;
using Optical_View.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Optical_View.View.Control
{
    /// <summary>
    /// Conversion3DView.xaml 的交互逻辑
    /// </summary>
    public partial class Conversion3DView : UserControl
    {
        public Conversion3DView()
        {
            InitializeComponent();
            load();
        }

        private void load() {

            ConversionText.Text = Launch.Startupz_type.Path.Split("\\")[Launch.Startupz_type.Path.Split("\\").Length - 1];
            Path_Lab.Text = Launch.Startupz_type.Path;
            void FindFile(string dirPath) //参数dirPath为指定的目录
            {
                //在指定目录及子目录下查找文件,在listBox1中列出子目录及文件
                DirectoryInfo Dir = new DirectoryInfo(dirPath);
                try
                {
                    foreach (DirectoryInfo d in Dir.GetDirectories())//查找子目录
{
                        FindFile(d.ToString() + "\\");
                        //listBox1.Items.Add(Dir + d.ToString() + "\"); //listBox1中填加目录名
                    }
                    foreach (FileInfo f in Dir.GetFiles("*.*")) //查找文件
                    {
                        //listBox1.Items.Add(Dir + f.ToString()); //listBox1中填加文件名
                        switch (System.IO.Path.GetExtension(f.FullName).ToLower())
                        {
                            case ".fbx":
                            case ".obj":
                            case ".glb":
                            case ".gltf":
                            case ".b3dm":
                            case ".osgb":
                                ListViewItem lvi = new ListViewItem() {
                                    Height = 69
                                };
                                var grid = new Grid()
                                {
                                    Width = 621,
                                    Height = 29
                                };

                                var progressBar = new ProgressBar()
                                {
                                    //MaterialDesignThemes.Wpf.TransitionAssist.DisableTransitions = true,
                                    Height = 69,
                                    Margin = new Thickness(-8, -21, 0, -19),
                                    Foreground = new SolidColorBrush(Color.FromArgb(100, 81, 45, 168)),
                                    Value = 0,
                                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                                };
                                var textBlock_name = new TextBlock()
                                {
                                    Style = (Style)this.FindResource("textBlockStyle"),
                                    TextTrimming = TextTrimming.CharacterEllipsis,
                                    Text = System.IO.Path.GetFileNameWithoutExtension(f.FullName),
                                    Margin = new Thickness(110, 0, 7, 0),
                                    FontSize = 14
                                };
                                var textBlock_path = new TextBlock()
                                {
                                    Style = (Style)this.FindResource("textBlockStyle"),
                                    TextTrimming = TextTrimming.CharacterEllipsis,
                                    Text = f.FullName,
                                    Margin = new Thickness(110, 18, 7, 0),
                                    FontSize = 8
                                };
                                var textBlock_type = new TextBlock()
                                {
                                    TextAlignment = TextAlignment.Center,
                                    Style = (Style)this.FindResource("textBlockStyle"),
                                    TextTrimming = TextTrimming.CharacterEllipsis,
                                    Text = System.IO.Path.GetExtension(f.FullName).ToUpper(),
                                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Width = 86,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    FontSize = 14
                                };
                                var materialDesign_Card = new MaterialDesignThemes.Wpf.Card()
                                {

                                    //materialDesign.ShadowAssist.ShadowDepth = "Depth1"
                                    Padding = new Thickness(32),
                                    Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204)),
                                    Margin = new Thickness(0, 0, 512, 0),
                                };
                                var button = new Button()
                                {
                                    Style = (Style)this.FindResource("MaterialDesignOutlinedButton"),
                                    ToolTip = "转换方向 顶点压缩 双面光等",
                                    Content = "设置",
                                    Margin = new Thickness(562, -12, 0, 17),
                                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                                    BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                                    FontSize = 12,
                                    Height = 24,
                                    Width = 59
                                };
                                grid.Children.Add(progressBar);
                                grid.Children.Add(textBlock_name);
                                grid.Children.Add(textBlock_path);
                                grid.Children.Add(textBlock_type);
                                //grid.Children.Add(materialDesign_Card);
                                grid.Children.Add(button);
                                lvi.Content = grid;
                                //grid.MouseUp += _record_MouseUp;
                                ListView.Items.Add(lvi);
                                break;
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            FindFile(Launch.Startupz_type.Path);
        }


        private void ConversionStart_Button_Click(object sender, RoutedEventArgs e)
        {
          
            void FindFile(string dirPath) //参数dirPath为指定的目录
            {
                //在指定目录及子目录下查找文件,在listBox1中列出子目录及文件
                DirectoryInfo Dir = new DirectoryInfo(dirPath);
                try
                {
                    foreach (DirectoryInfo d in Dir.GetDirectories())//查找子目录
                    {
                        FindFile(d.ToString() + "\\");
                    }
                    foreach (FileInfo f in Dir.GetFiles("*.*")) //查找文件
                    {
                        switch (System.IO.Path.GetExtension(f.FullName).ToLower())
                        {
                            case ".obj":
                                Task.Factory.StartNew(() =>
                                {
                                    string pathName = f.FullName.Split("\\")[f.FullName.Split("\\").Length - 2];
                                    if (!Directory.Exists(@"WebGL\.cache\" + pathName))
                                    {
                                        Directory.CreateDirectory(@"WebGL\.cache\" + pathName);
                                    }
                                    string copy_path = @"WebGL\.cache\" + pathName + @"\";

                                    string tileset = new objTob3dm().Obj_WriteTileset(f.FullName, copy_path);
                                    string b3dm = $"{copy_path}\\{ System.IO.Path.GetFileNameWithoutExtension(f.FullName)}.b3dm";

                                    Dispatcher.BeginInvoke(new Action(delegate
                                    {
                                        BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"Logic.add3DTiles('..{tileset.Replace("\\", "/").Replace("WebGL", "")}');");
                                    }));
                                });
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            FindFile(Launch.Startupz_type.Path);

        }
        string cache_Copy(string path,string file) {

            var pathName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (!Directory.Exists(@"WebGL\.cache\" + pathName))
            {
                Directory.CreateDirectory(@"WebGL\.cache\" + pathName);
            }

            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之

            var copy_path = @"WebGL\.cache\" + pathName + @"\" + System.IO.Path.GetFileName(file);
            File.Copy(file, copy_path, isrewrite);

            return copy_path;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _.Visibility = Visibility.Hidden;
            var @this = MainForm.control;

            TopBder.MouseMove += new MouseEventHandler(_MouseMove);
            void _MouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (@this != null) @this.DragMove();
                }
            }
            Popup_Close.Click += new RoutedEventHandler(Button_Click);


            grid_set_up.Visibility = Visibility.Visible;
            grid_set_up.Margin = new Thickness(0, 0, 0, 0);
            void Button_Click(object sender, RoutedEventArgs e)
            {
                grid_set_up.Visibility = Visibility.Hidden;
                grid_set_up.Margin = new Thickness(1906, 0, -1906, 0);

                _.Visibility = Visibility.Visible;
            }

        }
    }
}