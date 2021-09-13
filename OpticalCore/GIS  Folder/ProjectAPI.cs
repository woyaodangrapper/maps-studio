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
            DirectoryInfo root = new DirectoryInfo(path);

            foreach (DirectoryInfo f in root.GetDirectories())
            {
                fileName.Add(f.Name, GetFile(f.FullName));
                List<String> temporary = new List<String>();//临时存储
                if (Directory.Exists(f.FullName + "\\tiles"))
                {

                    DirectoryInfo png = new DirectoryInfo(f.FullName + "\\tiles");//获取切图数据
                    foreach (var item in png.GetDirectories())//循环获取切图数据子目录
                    {
                        if (item.Name == "L01")
                        {
                            temporary.Clear();
                            temporary.Add((f.FullName + "\\tiles").Replace(Route, NginxPath).Replace("\\", "/"));
                            break;
                        }
                        temporary.Add(item.FullName.Replace(Route, NginxPath).Replace("\\", "/"));
                    }
                }
                pngfileName.Add(f.Name, temporary.ToArray());

            }

            List<Object> oj = new List<Object>();

            foreach (var item in fileName.Keys)
            {
                List<Build> builds = Getbuild(fileName[item], item.ToString());
                List<Ztree> ztrees = GetZtrees(builds);

                object v = new
                {
                    fileName = item.ToString(),
                    Routefile = new//3D模型贴图
                    {

                        file3Dtiles = ztrees,//3Dtiles
                        fileimgtiles = pngfileName[item]//底图
                    }
                };
                oj.Add(v);
            }

            var file = new
            {
                datas = oj.ToArray()
            };


            return JsonConvert.SerializeObject(file);
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

        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="filename">数据源</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static List<Build> Getbuild(string[] filename, string name)
        {

            List<Build> builds = new List<Build>();

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
                        type = test.Substring(0, test.IndexOf("/"));
                        if (type == "JZ")
                        {       //JZ/1/SW
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
                        else if (type == "SNJZ")
                        {
                            set = "SN";
                            notype = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1); //Ao3/1
                            buildid = notype.Substring(0, notype.IndexOf("/"));
                            floor = notype.Substring(notype.IndexOf("/") + 1, notype.Length - notype.IndexOf("/") - 1);
                            type = "JZ";

                        }
                        else if (type.IndexOf("号楼") != -1)
                        {
                            buildid = type.Substring(0, type.IndexOf("号楼"));
                            nobuildid = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            set = nobuildid.Substring(0, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                            if (set == "SW")
                            {
                                if (nobuildid.IndexOf("/") == -1)
                                {
                                    floor = "";
                                }
                                else
                                {
                                    floor = nobuildid.Substring(nobuildid.IndexOf("/") + 1, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                                }
                            }
                            else
                            {
                                floor = nobuildid.Substring(nobuildid.IndexOf("/") + 1, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                            }
                            type = "JZ";
                        }

                        else if (type == "SN")
                        {
                            set = type;
                            noset = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            floor = noset.Substring(noset.IndexOf("/") + 1, noset.Length - noset.IndexOf("/") - 1);
                            buildid = noset.Substring(0, noset.IndexOf("/"));
                            type = "JZ";
                        }
                        else if (type == "SW")
                        {
                            set = type;
                            noset = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            buildid = noset.Substring(0, noset.IndexOf("/"));
                            floor = noset.Substring(noset.IndexOf("/") + 1, noset.Length - noset.IndexOf("/") - 1);
                            type = "JZ";
                        }
                        else if (type == "DXS")
                        {
                            floor = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
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
                        if (!String.IsNullOrEmpty(txt)) {
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


        /// <summary>
        /// 转换成ztree格式
        /// </summary>
        /// <param name="builds"></param>
        /// <returns></returns>
        public static List<Ztree> GetZtrees(List<Build> builds)
        {
            List<Ztree> zt = new List<Ztree>() {
             new Ztree("1","0","DX",""),
             new Ztree("2","0","JZ",""),
             new Ztree("3","0","XP",""),
             new Ztree("4","0","LH",""),
             new Ztree("5","0","SWJZ","")
            };
            int count = 6;
            foreach (var bitem in builds)
            {
                Ztree roots = new Ztree();
                Ztree node = new Ztree();
                Ztree ztree = zt.Find((Ztree z) => z.name.Equals(bitem.type));
                if (ztree == null)
                {
                    roots.id = count.ToString();
                    roots.pId = "0";
                    roots.name = bitem.type;
                    roots.site = "";
                    zt.Add(roots);
                    count++;
                    zt.Add(new Ztree(bitem.id, roots.id, bitem.floor, bitem.site));
                }
                else
                {
                    switch (bitem.type)
                    {
                        case "DX":
                        case "XP":
                        case "LH":
                        case "SWJZ":
                            string pid = "1";

                            if (bitem.type == "XP")
                            {
                                pid = "3";
                            }
                            if (bitem.type == "LH")
                            {
                                pid = "4";
                            }
                            if (bitem.type == "SWJZ")
                            {
                                pid = "5";
                            }
                            node = new Ztree(bitem.id, pid, bitem.type, bitem.site);
                            zt.Add(node);
                            break;
                        case "JZ":
                            ztree = zt.Find((Ztree z) => z.name.Equals(bitem.bulidid));
                            if (ztree == null)
                            {
                                roots.id = count.ToString();
                                roots.pId = "2";
                                roots.name = bitem.bulidid;
                                roots.site = "";
                                zt.Add(roots);
                                count++;
                                Ztree nodes = new Ztree(count.ToString(), roots.id, "", "");

                                if (bitem.set == "SW" || bitem.set == "SN")
                                {

                                    Ztree SN = new Ztree(count.ToString(), roots.id, "SN", "");
                                    zt.Add(SN);
                                    count++;
                                    Ztree SW = new Ztree(count.ToString(), roots.id, "SW", "");
                                    zt.Add(SW);
                                    count++;
                                    if (bitem.set == "SW")
                                    {
                                        if (bitem.floor == "")
                                        {
                                            zt.Add(new Ztree(bitem.id, SW.id, bitem.floor, bitem.site));
                                        }
                                        else
                                        {
                                            zt.Add(new Ztree(bitem.id, SW.id, bitem.set, bitem.site));
                                        }
                                    }
                                    else
                                    {
                                        zt.Add(new Ztree(bitem.id, SN.id, bitem.floor, bitem.site));
                                    }


                                }
                                else
                                {
                                    zt.Add(new Ztree(bitem.id, roots.id, bitem.floor, bitem.site));
                                }

                            }
                            else
                            {
                                Ztree childnode = new Ztree(bitem.id, "", bitem.floor, bitem.site);
                                if (bitem.set == "SW")
                                {
                                    Ztree SW = zt.Find((Ztree z) => z.name.Equals(bitem.set) && z.pId == ztree.id);
                                    if (SW != null)
                                    {
                                        childnode.pId = SW.id;
                                        if (bitem.floor == "")
                                        {
                                            childnode.name = bitem.set;
                                        }
                                        zt.Add(childnode);
                                    }
                                }
                                else if (bitem.set == "SN")
                                {
                                    Ztree SN = zt.Find((Ztree z) => z.name.Equals(bitem.set) && z.pId == ztree.id);
                                    if (SN != null)
                                    {
                                        childnode.pId = SN.id;
                                        zt.Add(childnode);
                                    }
                                }
                                else
                                {
                                    zt.Add(new Ztree(bitem.id, ztree.id, bitem.floor, bitem.site));
                                }




                            }
                            break;
                        default:
                            node = new Ztree(bitem.id, roots.id, bitem.floor, bitem.site);
                            if (bitem.floor == "")
                            {
                                node = new Ztree(bitem.id, roots.id, bitem.type, bitem.site);
                            }
                            zt.Add(node);
                            break;
                    }
                }

            }


            return zt;

        }

    }
    public class Linux_Gis
    {

        static String NginxPath = null;//替换外链路径
        static String Route = null;
        /// <summary>
        /// 获取到模型路径及值
        /// </summary>
        /// <returns></returns>
        // GET api/values
        public static string Getjson(string pathc)
        {
            String PathName = pathc.Split('/')[pathc.Split('/').Length - 1];
            Route = pathc;
            NginxPath = "http://" + "127.0.0.1" + ":9731/";
            if (String.IsNullOrEmpty(Route) || String.IsNullOrEmpty(NginxPath))
            {
                return "初始化失败，配置文件获取异常根目录未取到.";
            }

            Dictionary<String, String[]> fileName = new Dictionary<String, String[]>();
            Dictionary<String, String[]> pngfileName = new Dictionary<String, String[]>();
            string path = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(Route));
            DirectoryInfo root = new DirectoryInfo(path);

            foreach (DirectoryInfo f in root.GetDirectories())
            {
                {
                    fileName.Add(f.Name, GetFile(f.FullName));
                    List<String> temporary = new List<String>();//临时存储
                    if (Directory.Exists(f.FullName + "/tiles"))
                    {

                        DirectoryInfo png = new DirectoryInfo(f.FullName + "/tiles");//获取切图数据
                        foreach (var item in png.GetDirectories())//循环获取切图数据子目录
                        {
                            if (item.Name == "L01")
                            {
                                temporary.Clear();
                                temporary.Add((f.FullName + "/tiles").Replace(Route, NginxPath).Replace("\\", "/"));
                                break;
                            }
                            temporary.Add(item.FullName.Replace(Route, NginxPath).Replace("\\", "/"));
                        }
                    }
                    pngfileName.Add(f.Name, temporary.ToArray());
                }

            }

            List<Object> oj = new List<Object>();

            foreach (var item in fileName.Keys)
            {
                List<Build> builds = Getbuild(fileName[item], item.ToString());
                List<Ztree> ztrees = GetZtrees(builds);

                object v = new
                {
                    fileName = item.ToString(),
                    Routefile = new//3D模型贴图
                    {

                        file3Dtiles = ztrees,//3Dtiles
                        fileimgtiles = pngfileName[item]//底图
                    }
                };
                oj.Add(v);
            }

            var file = new
            {
                datas = oj.ToArray()
            };


            return JsonConvert.SerializeObject(file);
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
                    string pathNode = fullPath + "/" + dir[i].Name;
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
                        bl = fileList[i].Split('/')[fileList[i].Split('/').Length - 1].Split('.')[0] != "tileset" ? false : true;
                    }
                    else
                    {
                        bl = fileList[i].Split('.')[0] != "tileset" ? false : true;
                    }

                    if (bl)
                    {
                        a.Add(fileList[i].Replace(Route, NginxPath).Replace("/", "/"));
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
                string pathNodeB = path + "/" + dir[j].Name;
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

        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="filename">数据源</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static List<Build> Getbuild(string[] filename, string name)
        {

            List<Build> builds = new List<Build>();

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
                        type = test.Substring(0, test.IndexOf("/"));
                        if (type == "JZ")
                        {       //JZ/1/SW
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
                        else if (type == "SNJZ")
                        {
                            set = "SN";
                            notype = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1); //Ao3/1
                            buildid = notype.Substring(0, notype.IndexOf("/"));
                            floor = notype.Substring(notype.IndexOf("/") + 1, notype.Length - notype.IndexOf("/") - 1);
                            type = "JZ";

                        }
                        else if (type.IndexOf("号楼") != -1)
                        {
                            buildid = type.Substring(0, type.IndexOf("号楼"));
                            nobuildid = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            set = nobuildid.Substring(0, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                            if (set == "SW")
                            {
                                if (nobuildid.IndexOf("/") == -1)
                                {
                                    floor = "";
                                }
                                else
                                {
                                    floor = nobuildid.Substring(nobuildid.IndexOf("/") + 1, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                                }
                            }
                            else
                            {
                                floor = nobuildid.Substring(nobuildid.IndexOf("/") + 1, nobuildid.Length - nobuildid.IndexOf("/") - 1);
                            }
                            type = "JZ";
                        }

                        else if (type == "SN")
                        {
                            set = type;
                            noset = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            floor = noset.Substring(noset.IndexOf("/") + 1, noset.Length - noset.IndexOf("/") - 1);
                            buildid = noset.Substring(0, noset.IndexOf("/"));
                            type = "JZ";
                        }
                        else if (type == "SW")
                        {
                            set = type;
                            noset = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
                            buildid = noset.Substring(0, noset.IndexOf("/"));
                            floor = noset.Substring(noset.IndexOf("/") + 1, noset.Length - noset.IndexOf("/") - 1);
                            type = "JZ";
                        }
                        else if (type == "DXS")
                        {
                            floor = test.Substring(test.IndexOf("/") + 1, test.Length - test.IndexOf("/") - 1);
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


        /// <summary>
        /// 转换成ztree格式
        /// </summary>
        /// <param name="builds"></param>
        /// <returns></returns>
        public static List<Ztree> GetZtrees(List<Build> builds)
        {
            List<Ztree> zt = new List<Ztree>() {
             new Ztree("1","0","DX",""),
             new Ztree("2","0","JZ",""),
             new Ztree("3","0","XP",""),
             new Ztree("4","0","LH",""),
             new Ztree("5","0","SWJZ","")
            };
            int count = 6;
            foreach (var bitem in builds)
            {
                Ztree roots = new Ztree();
                Ztree node = new Ztree();
                Ztree ztree = zt.Find((Ztree z) => z.name.Equals(bitem.type));
                if (ztree == null)
                {
                    roots.id = count.ToString();
                    roots.pId = "0";
                    roots.name = bitem.type;
                    roots.site = "";
                    zt.Add(roots);
                    count++;
                    zt.Add(new Ztree(bitem.id, roots.id, bitem.floor, bitem.site));
                }
                else
                {
                    switch (bitem.type)
                    {
                        case "DX":
                        case "XP":
                        case "LH":
                        case "SWJZ":
                            string pid = "1";

                            if (bitem.type == "XP")
                            {
                                pid = "3";
                            }
                            if (bitem.type == "LH")
                            {
                                pid = "4";
                            }
                            if (bitem.type == "SWJZ")
                            {
                                pid = "5";
                            }
                            node = new Ztree(bitem.id, pid, bitem.type, bitem.site);
                            zt.Add(node);
                            break;
                        case "JZ":
                            ztree = zt.Find((Ztree z) => z.name.Equals(bitem.bulidid));
                            if (ztree == null)
                            {
                                roots.id = count.ToString();
                                roots.pId = "2";
                                roots.name = bitem.bulidid;
                                roots.site = "";
                                zt.Add(roots);
                                count++;
                                Ztree nodes = new Ztree(count.ToString(), roots.id, "", "");

                                if (bitem.set == "SW" || bitem.set == "SN")
                                {

                                    Ztree SN = new Ztree(count.ToString(), roots.id, "SN", "");
                                    zt.Add(SN);
                                    count++;
                                    Ztree SW = new Ztree(count.ToString(), roots.id, "SW", "");
                                    zt.Add(SW);
                                    count++;
                                    if (bitem.set == "SW")
                                    {
                                        if (bitem.floor == "")
                                        {
                                            zt.Add(new Ztree(bitem.id, SW.id, bitem.floor, bitem.site));
                                        }
                                        else
                                        {
                                            zt.Add(new Ztree(bitem.id, SW.id, bitem.set, bitem.site));
                                        }
                                    }
                                    else
                                    {
                                        zt.Add(new Ztree(bitem.id, SN.id, bitem.floor, bitem.site));
                                    }


                                }
                                else
                                {
                                    zt.Add(new Ztree(bitem.id, roots.id, bitem.floor, bitem.site));
                                }

                            }
                            else
                            {
                                Ztree childnode = new Ztree(bitem.id, "", bitem.floor, bitem.site);
                                if (bitem.set == "SW")
                                {
                                    Ztree SW = zt.Find((Ztree z) => z.name.Equals(bitem.set) && z.pId == ztree.id);
                                    if (SW != null)
                                    {
                                        childnode.pId = SW.id;
                                        if (bitem.floor == "")
                                        {
                                            childnode.name = bitem.set;
                                        }
                                        zt.Add(childnode);
                                    }
                                }
                                else if (bitem.set == "SN")
                                {
                                    Ztree SN = zt.Find((Ztree z) => z.name.Equals(bitem.set) && z.pId == ztree.id);
                                    if (SN != null)
                                    {
                                        childnode.pId = SN.id;
                                        zt.Add(childnode);
                                    }
                                }
                                else
                                {
                                    zt.Add(new Ztree(bitem.id, ztree.id, bitem.floor, bitem.site));
                                }




                            }
                            break;
                        default:
                            node = new Ztree(bitem.id, roots.id, bitem.floor, bitem.site);
                            if (bitem.floor == "")
                            {
                                node = new Ztree(bitem.id, roots.id, bitem.type, bitem.site);
                            }
                            zt.Add(node);
                            break;
                    }
                }

            }


            return zt;

        }

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
    public class Ztree
    {


        public string id { get; set; }

        public string pId { get; set; }

        public string name { get; set; }

        public string site { get; set; }

        public Ztree() { }

        public Ztree(string id, string pid, string name, string site)
        {
            this.id = id;
            this.pId = pid;
            this.name = name;
            this.site = site;
        }
    }
}
