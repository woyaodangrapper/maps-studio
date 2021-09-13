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
            public static Window control { get; set; }
        }

        //MainWindow
        public partial class BrowserContainer
        {
            public static View.Control.MicrosoftEdgeView control { get; set; }
        }
        //MainWindow
        public partial class ConversionView
        {
            public static View.Control.Conversion3DView control { get; set; }
        }
    }   
    
}
