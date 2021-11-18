using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_View.Model;
using Serilog;
using System;
using System.IO;
using static Optical_View.Model.View_static_control;

namespace Optical_View.Class
{
    public class BrowserMessageModel
    {
        public string name { get; set; }
        public string value { get; set; }

    }
    public class Browser_execution_method
    {

        WebsocketServer a = new WebsocketServer();
        //初始化模型
        private static void _buildinge_Stratification()
        {
            //JObject j = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Core.Main.GetDataModel(Launch.Startupz_type.Path)));
            //String data = JsonConvert.SerializeObject(Core.Main.GetDataModel(Launch.Startupz_type.Path));
            //BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"GIS.Socket_Response=" + data);

            //BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"Logic.example_BuildingStratification('{Launch.Startupz_type.Path.Replace("\\", "\\\\")}')");
        }
        private static void _extract_Loaded()
        {
            BrowserContainer.control.webView.CoreWebView2.ExecuteScriptAsync($"Logic.example_addBaseLayer();_water();");
        }
        private static void _obj_Three_add()
        {
            if (!Directory.Exists(@"WebGL\.cache"))
            {
                Directory.CreateDirectory(@"WebGL\.cache");
            }

            var obj = Launch.Startupz_type.Path;
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
        public static void WebSocketInit()
        {
            //new Core.Main().Init(new object[0]);
            //new WebsocketServer().WebSocketInit(_Message_processing);
        }

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
                                          switch (Launch.Startupz_type.Type)
                                          {
                                              case "obj":
                                                  try
                                                  {
                                                      _obj_Three_add();
                                                  }
                                                  catch { }
                                                  break;
                                              case "_folder":
                                                  _buildinge_Stratification();
                                                  break;

                                              case "_extract":
                                                  _extract_Loaded();
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
