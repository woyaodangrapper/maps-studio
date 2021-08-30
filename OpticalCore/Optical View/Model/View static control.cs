using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Optical_View.Model
{
    class View_static_control
    {
        //MainWindow
        public partial class MainForm
        {
            public MainForm(ref Window e)
            {
                control = e;
            }
            public static Window control { get; set; }
        }
    }
}
