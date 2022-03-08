using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_View.Model;
using Serilog;
using System;
using System.IO;
using static Optical_View.Model.ViewStaticMod;

namespace Optical_View.Class
{
    public class BrowserMessageModel
    {
        public string name { get; set; }
        public string value { get; set; }

    }
    /// <summary>
    /// WBB GL 交互类
    /// </summary>
    public class BrowserCS
    {


        /// <summary>
        /// CesiumGS添加建筑模型
        /// </summary>
        private static void _CesiumGS_buildinge_Stratification()
        {
            String data = JsonConvert.SerializeObject(new Optical_Core().GetStructure(LaunchMod.HistoricalProject.Path));
            //BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"GIS.Socket_Response=" + data);
            //BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"Logic.example_BuildingStratification('{Launch.HistoricalProject.Path.Replace("\\", "\\\\")}')");
        }

        /// <summary>
        /// Three添加模型
        /// </summary>
        private static void _Three_add()
        {
            if (!Directory.Exists(@"WebGL\.cache"))
            {
                Directory.CreateDirectory(@"WebGL\.cache");
            }

            var obj = LaunchMod.HistoricalProject.Path;
            {
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                File.Copy(obj, @"WebGL\.cache\" + Path.GetFileNameWithoutExtension(obj) + ".obj", isrewrite);



                string URI_ = "URI_" + Path.GetFileNameWithoutExtension(obj);
                string Img_Path = @"WebGL\.cache\" + URI_ + @"\";
                if (!Directory.Exists(Img_Path))
                {
                    Directory.CreateDirectory(Img_Path);
                }


                CommonOpenFileDialog mtl = new CommonOpenFileDialog("设置贴图");
                mtl.Filters.Add(new CommonFileDialogFilter("MTL Files", "*.mtl"));


                if (mtl.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    File.Copy(mtl.FileName, @"WebGL\.cache\" + Path.GetFileNameWithoutExtension(mtl.FileName) + ".mtl", isrewrite);
                    var dialogs = new CommonOpenFileDialog("设置图片资源文件夹");
                    dialogs.IsFolderPicker = true;
                    CommonFileDialogResult result = dialogs.ShowDialog();
                    if (result == CommonFileDialogResult.Ok)
                    {
                        var file = dialogs.FileName;


                        DirectoryInfo root = new DirectoryInfo(file);
                        FileInfo[] dics = root.GetFiles();
                        foreach (var f in dics)
                        {
                            switch (Path.GetExtension(f.FullName).ToLower())
                            {
                                case ".png":
                                case ".jpg":
                                case ".PNG":
                                case ".JPG":
                                case ".gif":
                                case ".GIF":
                                    File.Copy(f.FullName, Img_Path + Path.GetFileName(f.FullName), isrewrite);
                                    break;
                            }
                        }
                        Log.Information($"Loaded_Obj('{Path.GetFileNameWithoutExtension(obj)}','{Path.GetFileNameWithoutExtension(mtl.FileName)}','{URI_}')");
                        BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"Loaded_Obj('{Path.GetFileNameWithoutExtension(obj)}','{Path.GetFileNameWithoutExtension(mtl.FileName)}','{URI_}')");

                    }

                }
            }
        }


        /// <summary>
        /// 初始化WebSocket
        /// </summary>
        public static void WebSocketInit()
        {
            new SocketCS().WebSocketInit(_Message_processing);
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="bmm"></param>
        private static void _Message_processing(BrowserMessageModel bmm)
        {
            _ = MainForm.control.Dispatcher.BeginInvoke(new Action(delegate
              {
                  switch (bmm.name)
                  {
                      case "WebsocketServer":
                          JObject J = JsonConvert.DeserializeObject<JObject>(bmm.value);
                          switch (J["type"].ToString())
                          {
                              case "client":
                                  switch (J["msg"].ToString())
                                  {
                                      case "success":
                                          switch (LaunchMod.HistoricalProject.Type)
                                          {
                                              case "obj":
                                                  try
                                                  {
                                                      _Three_add();
                                                  }
                                                  catch { }
                                                  break;
                                              case "_folder":
                                                  _CesiumGS_buildinge_Stratification();
                                                  break;

                                              case "_extract":

                                                  break;
                                          }
                                          break;
                                  }
                                  break;
                              case "error":
                                  Log.Error("webView : " + J["msg"].ToString());
                                  BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"alert('{J["msg"].ToString()}')");
                                  break;
                          }
                          break;
                  }

              }));

        }

    }
}
