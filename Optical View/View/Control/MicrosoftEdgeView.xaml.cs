using Microsoft.Web.WebView2.Core;
using Optical_View.Class;
using Optical_View.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using static Optical_View.Model.StaticViewMod;

namespace Optical_View.View.Control
{
    /// <summary>
    /// MicrosoftEdgeView.xaml 的交互逻辑
    /// </summary>
    public partial class MicrosoftEdgeView : UserControl
    {
        EdgeViewCS EVCS;
        public MicrosoftEdgeView()
        {
            InitializeComponent();
            #region 初始化用户控件 扔入公共控件类
            EdgeView.Assembly = this;
            #endregion

            webView.Visibility = Visibility.Hidden;
            EVCS = new EdgeViewCS();
            EVCS.InitializeAsync();
            Visibility = Visibility.Visible;
        }

    }
}
