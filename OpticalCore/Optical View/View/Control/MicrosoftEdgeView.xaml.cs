using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Optical_View.View.Control
{
    /// <summary>
    /// MicrosoftEdgeView.xaml 的交互逻辑
    /// </summary>
    public partial class MicrosoftEdgeView : UserControl
    {
        public MicrosoftEdgeView()
        {
            InitializeComponent();
            InitializeAsync();
        }
        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            WebView_Loaded();
        }
        private void WebView_Loaded()
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.Source = new Uri("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }
      
    }
}
