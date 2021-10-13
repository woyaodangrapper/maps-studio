using Microsoft.Web.WebView2.Core;
using Optical_View.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using static Optical_View.Model.View_static_control;

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

            switch (Launch.Startupz_type.Type)
            {
                case "obj":
                    _obj_Loaded();
                    break;
                case "_folder":
                    _folder_Loaded();
                    break;
                case "_extract":
                    _extract_Loaded();
                    break;
            }
            #region 初始化用户控件 扔入公共控件类
            BrowserContainer.control = this;
            #endregion
        }
        private void _extract_Loaded()
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                webView.CoreWebView2.ContentLoading += CoreWebView2_ContentLoading;
                webView.Source = new Uri("http://127.0.0.1:" + Model.Web_Server_Config.Web_Server_Port.ToString() + "/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
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
                        Progress.control.Visibility = Visibility.Visible;
                    }));
                });
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }
        private void _folder_Loaded()
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                webView.CoreWebView2.ContentLoading += CoreWebView2_ContentLoading;
                webView.Source = new Uri("http://127.0.0.1:" + Model.Web_Server_Config.Web_Server_Port.ToString() + "/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
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
                        Progress.control.Visibility = Visibility.Visible;

                    }));
                });
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }
        private void _obj_Loaded()
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                webView.CoreWebView2.ContentLoading += CoreWebView2_ContentLoading;
                webView.Source = new Uri("http://127.0.0.1:" + Web_Server_Config.Web_Server_Port.ToString() + "/three/editor/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
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
                        Progress.control.Visibility = Visibility.Visible;
                    }));
                });
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }
        private void CoreWebView2_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            webView.CoreWebView2.ExecuteScriptAsync($" var s = setInterval(() => {{if (Logic.WebSocketint('{Model.Web_Server_Config.Web_WebsocketServer_Port.ToString()}')){{ clearInterval(s)}}}}, 1000);");

        }

    }
}
