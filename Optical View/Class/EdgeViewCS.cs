using Microsoft.Web.WebView2.Core;
using Optical_View.Model;
using System;
using System.Windows;
using System.Windows.Threading;
using static Optical_View.Model.StaticViewMod;

namespace Optical_View.Class
{
    /// <summary>
    /// 控件操作类
    /// </summary>
    class EdgeViewCS
    {
        Microsoft.Web.WebView2.Wpf.WebView2 webView = EdgeView.Assembly.webView;

        public async void InitializeAsync()
        {

            await webView.EnsureCoreWebView2Async(null);

            switch (ProjectMod.HistoricalProject.Type)
            {
                //case "obj":
                //    _obj_Loaded();
                //    break;
                case "_folder":
                    NetworkAgentCS.SetWebServer("CesiumGS");
                    _Loaded();
                    break;
                //case "_extract":
                //    _extract_Loaded();
                //    break;
            }
       
        }
 
        /// <summary>
        /// 初始化CesiumGS项目
        /// </summary>
        private void _Loaded()
        {
           
            if (webView != null && webView.CoreWebView2 != null)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                webView.CoreWebView2.ContentLoading += CoreWebView2_ContentLoading;
                webView.Source = new Uri("http://127.0.0.1:" + Model.NetworkAgentMod.Web_Server_Port.ToString() + "/index.html?time:" + Convert.ToInt64(ts.TotalSeconds).ToString());
                _ = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    int i = 0;
                    while (i <= 1)
                    {
                        i++;
                        System.Threading.Thread.Sleep(1000);
                    }
                    _ = Progress.Assembly.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        webView.Visibility = Visibility.Visible;
                        Progress.Assembly.Visibility = Visibility.Hidden;

                    }));
                });
                //webView.CoreWebView2.Navigate("http://127.0.0.1:" + Model.Web_Server_Config.Port.ToString());
            }
        }


        private void CoreWebView2_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            webView.CoreWebView2.ExecuteScriptAsync($" var s = setInterval(() => {{if (Logic.WebSocketint('{Model.NetworkAgentMod.Web_WebsocketServer_Port.ToString()}')){{ clearInterval(s)}}}}, 1000);");

        }


    }
}
