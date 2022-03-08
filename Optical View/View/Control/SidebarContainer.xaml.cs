using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Optical_View.Model.ViewStaticMod;

namespace Optical_View.View.Control
{

    /// <summary>
    /// Sidebar.xaml 的交互逻辑
    /// </summary>
    public partial class SidebarContainer : UserControl
    {

        public SidebarContainer()
        {
            InitializeComponent();

            SideUnfoldingGrid.Opacity = 0;
        }

        List<TreeViewItem> treeBox = new List<TreeViewItem>();
        static TreeViewItem mtrNode = new TreeViewItem();


        #region 界面动态逻辑

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            ////FaceButton
            ////ButtonBorder
            //FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 0, 122, 204));
            ////ButtonBorders.Background = new SolidColorBrush(Color.FromArgb(100, 0, 122, 204));
            //MessageBox.Show("hello");
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ENDBox.Background = new SolidColorBrush(Color.FromArgb(255, 20, 134, 211));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {

            ENDBox.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {//得到textbox控件焦点
            if (textBox.Text == "  搜索")
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 238));
                textBox.Text = null;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {//失去textbox控件焦点//135 135 135
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 135, 135, 135));
                textBox.Text = "  搜索";
            }
        }


        private void BorderT1_MouseEnter(object sender, MouseEventArgs e)
        {
            Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
            FaceButton.Background = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
        }

        private void BorderT1_MouseLeave(object sender, MouseEventArgs e)
        {
            FaceButton.Background = new SolidColorBrush(Color.FromArgb(255, 63, 63, 70));
            Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 182, 182, 182));
        }


        private void Monitor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SideUnfoldingGrid.Opacity == 100)
            {
                SideUnfoldingGrid.Opacity = 0;
                if (BrowserContainer.control != null)
                {


                    double left = BrowserContainer.control.Margin.Left - 405;
                    if (BrowserContainer.control.Name != "_")
                        BrowserContainer.control.Margin = new Thickness(left, 102, 0, 0);
                }
            }
            else
            {
                if (BrowserContainer.control != null)
                {
                    double left = BrowserContainer.control.Margin.Left + 405;
                    if (BrowserContainer.control.Name != "_")
                        BrowserContainer.control.Margin = new Thickness(left, 102, 0, 0);
                }
                SideUnfoldingGrid.Opacity = 100;
            }

        }

        private void ENDBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideUnfoldingGrid.Opacity = 0;
            if (BrowserContainer.control != null)
            {
                double left = BrowserContainer.control.Margin.Left - 405;
                if (BrowserContainer.control.Name != "_")
                    BrowserContainer.control.Margin = new Thickness(left, 102, 0, 0);
            }
        }



        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<TreeViewItem> cc = new List<TreeViewItem>();
            FileTree.Items.Clear();
            var treeBoxs = treeBox.Where(x => x.Header.ToString().Contains(textBox.Text == "  搜索" ? "" : textBox.Text)).ToList();//查询列表name
            foreach (var item in treeBoxs)
            {
                FileTree.Items.Add(item);
            }
            if (textBox.Text != "  搜索" && !String.IsNullOrEmpty(textBox.Text))
            {
                foreach (var item in treeBox)
                {
                    foreach (var items in item.Items)
                    {
                        TreeViewItem tvi = (TreeViewItem)items;
                        if (tvi.Header.ToString().Contains(textBox.Text == "  搜索" ? "" : textBox.Text))
                        {
                            cc.Add(tvi);
                        }
                    }
                }

                foreach (var item in cc)
                {
                    TreeViewItem tvi = (TreeViewItem)item;
                    FileTree.Items.Add(new TreeViewItem
                    {
                        Header = tvi.Header,
                        Style = tvi.Style,
                        Tag = tvi.Tag
                    });
                }
            }

        }
        #endregion
        private void FileTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem items = FileTree.SelectedItem as TreeViewItem;
            try
            {

                string result = items.Tag.ToString();
                if (!String.IsNullOrEmpty(result))
                {

                }

            }
            catch (Exception ex)
            {
                if (ex.Message != "未将对象引用设置到对象的实例。")
                {
                    //LoadingState.SetUpdateState("无法进行远程连接");
                }
            }
        }

    }
}
