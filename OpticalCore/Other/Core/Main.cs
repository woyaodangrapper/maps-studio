using InterFace;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_View.Class;
using Optical_View.Model;
using Serilog;
using System;
using System.IO;
using System.Net;

namespace Core
{
    public class Main : IExpand
    {
        static Windows_ImWebServer Server = new Windows_ImWebServer();
        public void Init(object[] obj)
        {
            #region 初始化LOG日志
            Web_Server_Config.Port = SystemOperation.PortIsUsed();
            {
                //Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Information()
                //.WriteTo.Console()
                //.MinimumLevel.Debug()
                //.WriteTo.File(Path.Combine("Core", DateTime.Now.ToString("yyyyMMdd") + $".txt"),
                //    rollingInterval: RollingInterval.Day,
                //    rollOnFileSizeLimit: true)
                //.CreateLogger();
                Log.Debug("Core - Init");
            }
            #endregion
        }
     
        public static string GIS_json { get; set; } = "{}";
        public static object GetDataModel(String path) {
            Server.stop();
           
            try
            {
                Server.start(IPAddress.Parse("127.0.0.1"), Optical_View.Model.Web_Server_Config.Port, 1, path);
                GIS_json = Windows_Gis.Getjson(path);
                return (new
                {
                    list = JsonConvert.DeserializeObject<JObject>(GIS_json)
                });
            }
            catch (Exception ex)
            {
                return (new
                {
                    messerr = ex.Message
                });
            }
           
        }
       
    }
}
