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
            webView.Visibility = Visibility.Hidden;
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
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                webView.Source = new Uri("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString() + "/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
                _ = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    int i = 0;
                    while (i <= 1)
                    {
                        i++;
                        System.Threading.Thread.Sleep(1000);
                    }
                    _ = Dispatcher.BeginInvoke(new Action(delegate
                    {
                        webView.Visibility = Visibility.Visible;
                    }));
                });
             
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }
      
    }
}
