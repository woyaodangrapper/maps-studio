using ConsoleServer.IInterFace;
using InterFace;
using OpticalCore.MQTT;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;

namespace OpticalCore
{

    class Program
    {
        //static ConfigurationBuilder builder = (ConfigurationBuilder)new ConfigurationBuilder()
        //   .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
        //   .AddJsonFile("appsettings.json");
        //static IConfiguration configuration = builder.Build();
        static FunctionClass fc = new FunctionClass();

        #region 关闭快捷编辑
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);
        #endregion
        #region 初始化拓展
        public static Dictionary<String, ObtainInterFace.plugins> Method = ObtainInterFace.ObtainPlugins("IExpand");
        #endregion

        static void Main(string[] args)
        {
            #region 防止控制台输出堵塞
            void DisbleQuickEditMode()
            {
                IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
                uint mode;
                GetConsoleMode(hStdin, out mode);
                mode &= ~ENABLE_QUICK_EDIT_MODE;
                SetConsoleMode(hStdin, mode);

            }
            DisbleQuickEditMode();
            #endregion

            #region 重启
            /// <summary>
            /// 设置程序重启
            /// </summary>
            void InitRestart()
            {
                Timer cTimer = new Timer();
                cTimer.Interval = 3000;
                cTimer.Elapsed += Restart;
                //cTimer.AutoReset = false;
                cTimer.Start();
            }
            /// <summary>
            /// 重启自己
            /// </summary>
            void Restart(object sender, ElapsedEventArgs e)
            {
                //重启时间设定
                int h = DateTime.Now.Hour;
                int m = DateTime.Now.Minute;
                int s = DateTime.Now.Second;
                int h1 = 1;//2
                int m1 = 1;//0
                int s1 = 0;
                int s2 = 30;
                if (h == h1 && m == m1 && s >= s1 && s <= s2)
                {
                    Log.Debug("软件重启" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    // Restart current process Method 1

                    System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
            }
            InitRestart();
            #endregion

            #region NQSERVER(TT)
            //开启和webapi的MQ服务 
            Task.Run(IQTServer.RunAsync);
            #endregion

            #region 初始化LOG日志
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine("ConsoleServer", DateTime.Now.ToString("yyyyMM") + $".txt"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
            #endregion

            #region 清理控制台输出
            /// <summary>
            /// 设置程序清理
            /// </summary>
            void InitCW()
            {
                Timer cTimer = new Timer();
                cTimer.Interval = 1000 * 3600;
                cTimer.Elapsed += ConsoleClear;
                //cTimer.AutoReset = false;
                cTimer.Start();
            }
            /// <summary>
            /// 清理自己
            /// </summary>
            void ConsoleClear(object sender, ElapsedEventArgs e)
            {
                Console.Clear();
                Log.Debug("清理控制台完毕。");
            }
            InitCW();
            #endregion

            #region 初始化拓展
            if(Method != null)
                foreach (var item in Method.Keys)
                {
                    ((IExpand)Method[item].o).Init(new object[] { null });//
                }
            #endregion

            var input = Console.ReadLine();
            while (input != "exit")
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "查询接口列表":

                        break;
                    default:
                        foreach (var item in Program.Method.Keys)
                        {
                            var mod = Program.Method[item];
                            MethodInfo method = mod.type.GetMethod("ForwardEvent");// 控制台输入模拟报警
                            if (method != null)
                            {
                                object[] obj =
                                {
                                    input
                                };
                                //对方法进行调用
                                method.Invoke(mod.o, obj);//param为方法参数object数组
                            }
                        }
                        //MQTT_CDI.ForwardEvent(input);
                        break;
                }
            }
        }

        /// <summary>
        /// 日志回调
        /// </summary>
        class FunctionClass   //开发层处理，开发人员编写具体的计算方法
        {
            public void GETDLLMESS(IG_OV.IReturnParameters RP)
            {
                Log.Debug($"{RP.currentMethod},{RP.declaringType},{RP.substate},{RP.content}");
            }
        }
    }
}
