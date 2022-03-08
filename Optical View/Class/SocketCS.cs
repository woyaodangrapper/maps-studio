using Fleck;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Optical_View.Class
{
    /// <summary>
    /// 通信类
    /// </summary>
    public class SocketCS
    {
        static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static WebSocketServer server;

        public delegate void Work(BrowserMessageModel e);
        public void WebSocketInit(Work Work)
        {


            FleckLog.Level = LogLevel.Debug;
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            List<IPAddress> ipv4 = new List<IPAddress>();
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ipv4.Add(ipa);
            }
            Model.WebServerMod.Web_WebsocketServer_Port = Class.SystemCS.PortIsUsed();
            String potr = Model.WebServerMod.Web_WebsocketServer_Port.ToString();
            //server = new WebSocketServer($"ws://{ipv4[0]}:4649");//34.203.114.7
            Log.Information($"WebSocketInit: ws://127.0.0.1:{potr}");
            server = new WebSocketServer($"ws://127.0.0.1:{potr}");
            Log.Information("开始");
            try
            {
                Log.Debug("初始化WebSocketServer连接");
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}  => Open!");
                        allSockets.Add(socket);
                    };
                    socket.OnClose = () =>
                    {
                        Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}  => Close!");
                        allSockets.Remove(socket);
                    };
                    socket.OnMessage = message =>
                    {
                        Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}  => " + message);

                        var a = System.Reflection.MethodBase.GetCurrentMethod();
                        Work(new BrowserMessageModel
                        {
                            name = "WebsocketServer",
                            value = message
                        });
                        allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                    };
                });
            }
            catch (Exception)
            {
                IntPtr Exceptionstate = new IntPtr(0);
                while (Exceptionstate == new IntPtr(0))
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                        Console.BackgroundColor = ConsoleColor.Red; //设置背景色

                        Log.Debug($"Starting WebSocket and terminal communication");//正在启动 WebSocket和终端通信

                        Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                        Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                        Console.BackgroundColor = ConsoleColor.Yellow; //设置背景色

                        Log.Debug("重新设置本机IP地址：");

                        Log.Debug("等待....");
                        server = new WebSocketServer($"ws://127.0.0.1:" + potr);//34.203.114.7
                        server.Start(socket =>
                        {
                            socket.OnOpen = () =>
                            {
                                Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}  => Open!");
                                allSockets.Add(socket);
                            };
                            socket.OnClose = () =>
                            {
                                Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}  => Close!");
                                allSockets.Remove(socket);
                            };
                            socket.OnMessage = message =>
                            {
                                Log.Debug($"{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort} =>" + message);
                                Work(new BrowserMessageModel
                                {
                                    name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    value = message
                                });
                                allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                            };
                        });
                        Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                        Console.BackgroundColor = ConsoleColor.Green; //设置背景色

                        Log.Debug("重新定位IP地址 并且服务初始化完成。。。");

                        Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                        Exceptionstate = new IntPtr(1);

                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.Message);
                    }
                }

            }




            //var input = Console.ReadLine();
            //while (input != "exit")
            //{
            //    foreach (var socket in allSockets.ToList())
            //    {
            //        socket.Send(input);

            //        Log.Information($"Send:{input} =>{socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort}");
            //    }
            //    input = Console.ReadLine();
            //}

            //ImHelper.SendChanMessage();
        }
        public static void SetWebSocketMsg(byte[] msg)
        {
            if (server != null)
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Green; //设置背景色
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(msg);
                }
                Console.ResetColor(); //将控制台的前景色和背景色设为默认值

            }
            else
            {
                Log.Debug("The communication module is not initialized!");
            }
        }
        public static void SetWebSocketMsg(String msg)
        {
            if (server != null)
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(msg);
                }
                Log.Debug("WebSocket 4649:内置消息已同步转发出。。。");
            }
            else
            {
                Log.Debug("The communication module is not initialized!");
            }
        }
    }
}
