using Optical_View.View.Form;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static Optical_View.Model.View_static_control;

namespace Optical_View.View.Control
{
    /// <summary>
    /// HeaderContainer.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderContainer : UserControl
    {
        public HeaderContainer()
        {
            InitializeComponent();
        }

        private void LaunchView_Click(object sender, RoutedEventArgs e)
        {
            new Launch_Window().Show();
            MainForm.control.Close();
        }
    }
}
