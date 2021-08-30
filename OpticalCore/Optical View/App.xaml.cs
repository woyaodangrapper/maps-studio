
//using CefSharp;
//using CefSharp.Wpf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Optical_View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);
        public static void DoEvents()//强制刷新
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, exitFrameCallback, nestedFrame);
            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status !=
            DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }
        /**
         * 提高FPS(效果未知)
         * */
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 60 }//60、90、100 
                );
        }
        private static Object ExitFrame(Object state)
        {
         

            DispatcherFrame frame = state as
            DispatcherFrame;
            frame.Continue = false;
            return null;
        }

        public App()
        {
            #region 初始化LOG日志
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine("Optical Log", DateTime.Now.ToString("yyyyMMdd") + $".txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
            }
            #endregion


            // Add Custom assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            InitializeCefSharp();//Cef内核 因wpf webgl刷新问题不使用/但无法解决 除win10以下版本使用本工具
            InitializeWebServer(); //初始化内置的代理服务器
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            //var settings = new CefSettings();
            //settings.CommandLineArgsDisabled = false;
            //settings.CefCommandLineArgs.Clear();
            //settings.CefCommandLineArgs.Add("enable-3d-apis", "1");
            //settings.CefCommandLineArgs.Add("enable-webgl-draft-extensions", "1");
            //settings.CefCommandLineArgs.Add("enable-gpu", "1");
            //settings.CefCommandLineArgs.Add("enable-webgl", "1");
            //// Set BrowserSubProcessPath based on app bitness at runtime
            //settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            //                                       @$"runtimes\win-{(Environment.Is64BitProcess ? "x64" : "x86")}\lib\netcoreapp3.0\CefSharp.BrowserSubprocess.exe");

            //// Make sure you set performDependencyCheck false
            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }
       
        private static void InitializeWebServer()
        {
            Model.Web_Server_Config.Port = Class.SystemOperation.PortIsUsed();
            Log.Debug("Model.Web_Server_Config.Port =>" + Model.Web_Server_Config.Port);
            _ = Class.Windows_ImWebServer.start(IPAddress.Parse("127.0.0.1"), Model.Web_Server_Config.Port, 1, "WebGL");
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }
    }
}
