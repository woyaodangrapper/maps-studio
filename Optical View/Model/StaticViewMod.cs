using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Optical_View.Model
{
    class StaticViewMod
    {
        //MainWindow
        public partial class Main
        {
            public static Window Assembly { get; set; }
        }

        /// <summary>
        /// 浏览器
        /// </summary>
        public partial class EdgeView
        {
            public static View.Control.MicrosoftEdgeView Assembly { get; set; }
        }
        //MainWindow
        public partial class ConversionView
        {
            public static View.Control.Conversion3DView Assembly { get; set; }
        }
        //容器页面的进度条
        public partial class Progress
        {
            public static ProgressBar Assembly { get; set; }
        }
    }   
    
}
