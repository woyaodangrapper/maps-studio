using InterFace;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConsoleServer.IInterFace
{
    class ObtainInterFace
    {
        static List<Type> typeofList = new List<Type> {typeof(IExpand) };
        //public static Dictionary<String, object> IPluginList {get; set;}
        public class plugins {
            public  int index { get; set; }
            public  string name { get; set; }
            public  string pathName {get;set;}
            public  Assembly asm { get; set;}
            public  Type type { get; set; }
            public object o { get; set; }
        }
        public static Dictionary<string, plugins> ObtainPlugins(String IPluginName)
        {
           
            List<string> pluginpath = FindPlugin();
            List<string> interfacename = new List<string>();

          

            foreach (var item in typeofList)
            {
                interfacename.Add(item.FullName);
            }
            int index = 0;
            pluginpath = DeleteInvalidPlungin(pluginpath, interfacename.ToArray());
            Dictionary<String, plugins> Method = new Dictionary<String, plugins>();
            foreach (string asmfile in pluginpath)
            {
                try
                {
                    //获取文件名
                    string asmname = Path.GetFileNameWithoutExtension(asmfile);
                    if (asmname != string.Empty)
                    {
                        // 利用反射,构造DLL文件的实例
                        Assembly asm = Assembly.LoadFile(asmfile);
                        //利用反射,从程序集(DLL)中,提取类,并把此类实例化
                        Type[] t = asm.GetExportedTypes();
                        foreach (Type type in t)
                        {
                            if (type.GetInterface(IPluginName) != null)
                            {
                                object MethodList = (object)Activator.CreateInstance(type);
                                string pathName = asmfile.Split('\\')[asmfile.Split('\\').Length - 1];
                                Method.Add(pathName, new plugins {
                                    index = index,
                                    name = asmname,
                                    pathName = pathName,
                                    asm = asm,
                                    type = type,
                                    o = MethodList
                                });
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            //IPluginList = Method;
            return Method;
        }
        //查找所有插件的路径
        private static List<string> FindPlugin()
        {
            List<string> pluginpath = new List<string>();
            try
            {
                //获取程序的基目录
                string path = AppDomain.CurrentDomain.BaseDirectory;
                //合并路径，指向插件所在目录。
                path = Path.Combine(path, "");
                if (!Directory.Exists(path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    directoryInfo.Create();
                }
                foreach (string filename in Directory.GetFiles(path, "*.dll"))
                {
                    System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(filename);
                    //if (info.CompanyName == "OV_Mqtt")
                        pluginpath.Add(filename);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return pluginpath;
        }
        //载入插件，在Assembly中查找类型
        private static object LoadObject(Assembly asm, string className, string interfacename, object[] param)
        {
            try
            {
                //取得className的类型
                Type t = asm.GetType(className);
                if (t == null
                    || !t.IsClass
                    || !t.IsPublic
                    || t.IsAbstract
                    || t.GetInterface(interfacename) == null
                   )
                {
                    return null;
                }
                //创建对象
                Object o = Activator.CreateInstance(t, param);
                if (o == null)
                {
                    //创建失败，返回null
                    return null;
                }
                return o;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //移除无效的的插件，返回正确的插件路径列表，Invalid:无效的
        private static List<string> DeleteInvalidPlungin(List<string> PlunginPath, string[] interfacename)
        {
            List<string> rightPluginPath = new List<string>();
            //遍历所有插件。
            foreach (string filename in PlunginPath)
            {
                //Assembly asm = Assembly.Load(filename);
             
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(filename);
                        //遍历导出插件的类。
                        foreach (Type t in asm.GetExportedTypes())
                        {
                            foreach (var item in interfacename)
                            {
                                Object plugin = LoadObject(asm, t.FullName, item, null);
                                //如果找到，将插件路径添加到rightPluginPath列表里，并结束循环。
                                if (plugin != null)
                                {
                                    rightPluginPath.Add(filename);
                                    break;
                                }
                                //查找指定接口
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                       Log.Error(filename.Split('\\')[filename.Split('\\').Length - 1] + "不是有效插件" + ex.Message);
                    }
                }
                
            }

            return rightPluginPath;
        }
    }
}
