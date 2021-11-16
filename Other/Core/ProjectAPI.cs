using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Optical_View.Class
{
    public class Windows_Gis
    {

        static String NginxPath = null;//替换外链路径
        static String Route = null;

        public class FileNames
        {
            public int id { get; set; }
            public string uri { get; set; }

            public string text { get; set; }
            public state state { get; set; }
            public List<FileNames> children { get; set; }
            public string icon { get; set; }
        }
        public class state
        {
            public bool opened { get; set; }
        }
        //以上字段为树形控件中需要的属性
        //获得指定路径下所有文件名
        public static List<FileNames> getFileName(List<FileNames> list, string filepath)
        {
            DirectoryInfo root = new DirectoryInfo(filepath);
            foreach (FileInfo f in root.GetFiles())
            {
                var str = Path.GetFileName(f.FullName).ToLower();
                if ("tileset.json" == str)
                {
                    list.Add(new FileNames
                    {
                        uri = f.FullName.Replace(Route, NginxPath).Replace("\\", "/"),
                        text = f.Name,
                        state = new state { opened = false },
                        icon = "jstree-file"
                    });
                }
            }
            return list;
        }
        //获得指定路径下的所有子目录名
        // <param name="list">文件列表</param>
        // <param name="path">文件夹路径</param>
        public static List<FileNames> GetallDirectory(List<FileNames> list, string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            var dirs = root.GetDirectories();
            if (dirs.Count() != 0)
            {
                foreach (DirectoryInfo d in dirs)
                {
                    list.Add(new FileNames
                    {
                        text = d.Name,
                        state = new state { opened = false },
                        children = GetallDirectory(new List<FileNames>(), d.FullName)
                    });
                }
            }
            list = getFileName(list, path);
            return list;
        }
        /// <summary>
        /// 获取到模型路径及值
        /// </summary>
        /// <returns></returns>
        // GET api/values
        public static string Getjson(string pathc)
        {
            Route = pathc;
            NginxPath = "http://" + "127.0.0.1" + ":"+ Optical_View.Model.Web_Server_Config.Port + "/";
            if (String.IsNullOrEmpty(Route) || String.IsNullOrEmpty(NginxPath))
            {
                return "初始化失败，配置文件获取异常根目录未取到.";
            }

            Dictionary<String, String[]> fileName = new Dictionary<String, String[]>();
            Dictionary<String, String[]> pngfileName = new Dictionary<String, String[]>();
            string path = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(Route));// @"D:\Work\publish\MapData";


            List<FileNames> GetAllPath()
            {
                //获取当前系统的根路径           
                string rootpath = path;
                var list = GetallDirectory(new List<FileNames>(), rootpath).ToArray();
                return list.ToList();
            }
            var data = GetAllPath();

            
            var file = new
            {
                datas = data
            };
            return JsonConvert.SerializeObject(file);
        }
        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="filename">数据源</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static List<object> Getbuild(string[] filename, string name)
        {

            List<object> builds = new List<object>();

            try
            {
                foreach (var item in filename)
                {
                    string type = "";
                    string no3dtiles = "";
                    string nobuildid = "";
                    string buildid = "";
                    string set = "";
                    string floor = "";
                    string notype = "";
                    string test = "";
                    string noset = "";


                    if (item.IndexOf("3dtiles") == -1)
                    {
                        int dd = item.IndexOf(name) + name.ToString().Length + 1;
                        int bb = item.Length - dd;
                        no3dtiles = item.Substring(dd, bb);
                    }
                    else
                    {
                        no3dtiles = item.Substring(item.IndexOf("3dtiles") + 8, item.Length - item.IndexOf("3dtiles") - 8);
                    }
                    test = no3dtiles.Substring(0, no3dtiles.IndexOf("/tileset.json"));//JZ/1/SN/F1,DX,LH

                    if (test.IndexOf("/") != -1)// JZ/SN/F1
                    {
                      
                            notype = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);/// 1 / SW
                            buildid = notype.Substring(0, notype.IndexOf("/"));
                            nobuildid = notype.Substring(notype.IndexOf("/") + 1, notype.Length - 1 - notype.IndexOf("/"));
                            if (buildid == "SN")
                            {
                                buildid = "1";
                                set = buildid;
                                floor = nobuildid;
                            }
                            else
                            {
                                if (nobuildid.IndexOf("/") == -1)
                                {
                                    if (nobuildid == "SN" || nobuildid == "SW")
                                    {
                                        set = nobuildid;
                                    }
                                    else
                                    {
                                        floor = nobuildid;
                                    }

                                }
                                else
                                {
                                    set = nobuildid.Substring(0, nobuildid.IndexOf("/"));
                                    if (set != "SN" || set != "SW")
                                    {
                                        floor = nobuildid.Substring(nobuildid.IndexOf("/") + 1, nobuildid.Length - 1 - nobuildid.IndexOf("/")); ;
                                    }
                                }
                            }

                     
                    }
                    else
                    {
                        type = test;

                    }

                    if (set.IndexOf("/") != -1)
                    {
                        set = set.Replace("/", "");
                    }

                    Build b = new Build();
                    string uri = "http://";
                    for (int i = 0; i < item.Split("http://")[1].Split("/").Length; i++)
                    {
                        string txt = item.Split("http://")[1].Split("/")[i];
                        if (System.Text.RegularExpressions.Regex.IsMatch(txt, @"[\u4e00-\u9fbb]")) { txt = System.Web.HttpUtility.UrlEncode(txt); }
                        if (!String.IsNullOrEmpty(txt))
                        {
                            uri += txt + (i < item.Split("http://")[1].Split("/").Length - 1 ? "/" : "");
                        }
                    }
                    foreach (var items in item.Split("/"))

                    b.site = uri;
                    b.type = type;
                    b.bulidid = buildid;
                    b.set = set;
                    b.floor = floor;
                    b.id = Guid.NewGuid().ToString("N");
                    builds.Add(b);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex + " error:" + name);
            }

            return builds;
        }

        public class Build
        {
            public string id { get; set; }
            /// <summary>
            /// 数据类型
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 楼层编号
            /// </summary>
            public string bulidid { get; set; }


            /// <summary>
            /// 室内外
            /// </summary>
            public string set { get; set; }

            /// <summary>
            /// 楼层
            /// </summary>
            public string floor { get; set; }


            /// <summary>
            /// 数据
            /// </summary>
            public string site { get; set; }

        }
        #region 查找目录下包含子目录的全部文件


        public static List<string> fileList = new List<string>();
        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件地址  isFullName是确定文件名称还是路径
        /// </summary>
        public static string[] GetFile(string fullPath, bool isFullName = true)
        {
            try
            {
                fileList.Clear();

                DirectoryInfo dirs = new DirectoryInfo(fullPath); //获得程序所在路径的目录对象
                DirectoryInfo[] dir = dirs.GetDirectories();//获得目录下文件夹对象
                FileInfo[] file = dirs.GetFiles();//获得目录下文件对象
                int dircount = dir.Count();//获得文件夹对象数量
                int filecount = file.Count();//获得文件对象数量

                //循环文件夹
                for (int i = 0; i < dircount; i++)
                {
                    string pathNode = fullPath + "\\" + dir[i].Name;
                    GetMultiFile(pathNode, isFullName);
                }

                //循环文件
                for (int j = 0; j < filecount; j++)
                {
                    if (isFullName)
                    {
                        fileList.Add(file[j].FullName);
                    }
                    else
                    {
                        fileList.Add(file[j].Name);
                    }
                }
                List<String> a = new List<String>();
                //筛选字符过滤非3dtiles或其他
                for (int i = 0; i < fileList.Count; i++)
                {
                    bool bl = false;
                    if (isFullName)
                    {
                        bl = fileList[i].Split('\\')[fileList[i].Split('\\').Length - 1].Split('.')[0] != "tileset" ? false : true;
                    }
                    else
                    {
                        bl = fileList[i].Split('.')[0] != "tileset" ? false : true;
                    }

                    if (bl)
                    {
                        a.Add(fileList[i].Replace(Route, NginxPath).Replace("\\", "/"));
                    }

                }
                return a.ToArray();
            }
            catch (Exception ex)
            {
                // ex.Message + "\r\n出错的位置为：Form1.PaintTreeView()";
            }

            return null;
        }

        private static bool GetMultiFile(string path, bool isFullName = true)
        {
            if (Directory.Exists(path) == false)
            { return false; }

            DirectoryInfo dirs = new DirectoryInfo(path); //获得程序所在路径的目录对象
            DirectoryInfo[] dir = dirs.GetDirectories();//获得目录下文件夹对象
            FileInfo[] file = dirs.GetFiles();//获得目录下文件对象
            int dircount = dir.Count();//获得文件夹对象数量
            int filecount = file.Count();//获得文件对象数量
            int sumcount = dircount + filecount;

            if (sumcount == 0)
            { return false; }

            //循环文件夹
            for (int j = 0; j < dircount; j++)
            {
                string pathNodeB = path + "\\" + dir[j].Name;
                GetMultiFile(pathNodeB, isFullName);
            }

            //循环文件
            for (int j = 0; j < filecount; j++)
            {
                if (isFullName)
                {
                    fileList.Add(file[j].FullName);
                }
                else
                {
                    fileList.Add(file[j].Name);
                }
            }


            return true;
        }

        #endregion

   

    }
    
}
