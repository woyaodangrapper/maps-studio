using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optical_Core.Class;
using Optical_Core.Model;
using System;
using System.Net;

namespace Optical_View
{
    public class Optical_Core
    {
        public Optical_Core() {
            Web_Server_Config.Port = SystemOperation.PortIsUsed();
        }
        static Windows_ImWebServer Server = new Windows_ImWebServer();
     
        public static string GIS_json { get; set; } = "{}";
        public object GetStructure(String path) {
            Server.stop();
            try
            {
                Server.start(IPAddress.Parse("127.0.0.1"), Web_Server_Config.Port, 1, path);
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
