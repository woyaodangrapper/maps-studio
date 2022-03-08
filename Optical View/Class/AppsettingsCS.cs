using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Optical_View.Class
{
    /// <summary>
    /// 配置文件类
    /// </summary>
    class AppsettingsCS
    {
        private static ConfigurationBuilder builder;
        private static IConfiguration configuration;
        public AppsettingsCS()
        {
            //判断文件的存在
            if (!File.Exists("appsettings.json"))
            {
                File.Create("appsettings.json").Close();//创建该文件

                using (var fs = File.AppendText("appsettings.json"))
                {
                    fs.WriteLine("{}");
                }
            }
            void init()
            {

                builder = (ConfigurationBuilder)new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                  .AddJsonFile("appsettings.json");
                configuration = builder.Build();
            }
            try
            {
                init();
            }
            catch (Exception)
            {
                using (StreamWriter sw = new StreamWriter("appsettings.json", false))
                {
                    sw.Write("{}");

                }
                init();
            }

        }

        public JArray GetKeys(string key)
        {
            string txt = null;
            using (StreamReader sr = new StreamReader("appsettings.json"))
            {
                string line;
                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    txt += line;
                }
            }
            if (string.IsNullOrEmpty(txt)) { txt = "{}"; }
            var job = JsonConvert.DeserializeObject<JObject>(txt);

            if (job[key] == null)
            {
                return JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(new object[0]));
            }
            else
            {
                return JsonConvert.DeserializeObject<JArray>(job[key].ToString());
            }
        }

        public bool SetKeys(string key, JObject value)
        {
            string txt = null;
            using (StreamReader sr = new StreamReader("appsettings.json"))
            {
                string line;
                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    txt += line;
                }
            }
            if (string.IsNullOrEmpty(txt)) { txt = "{}"; }
            List<JToken> L = new List<JToken>();
            L.Add(value);
            var job = JsonConvert.DeserializeObject<JObject>(txt);
            if (job[key] != null)
            {
                foreach (var item in job[key])
                {
                    if (value.ToString() != item.ToString())
                    {
                        L.Add(item);
                    }

                }
            }

            job[key] = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(L.ToArray()));

            using (StreamWriter sw = new StreamWriter("appsettings.json", false))
            {
                sw.Write(JsonConvert.SerializeObject(job));

            }
            return true;
        }

    }
}
