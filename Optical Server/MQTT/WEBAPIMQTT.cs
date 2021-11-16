using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Extensions;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpticalServer.MQTT
{
    /// <summary>
    /// MQ客户端用于和中间件交互
    /// </summary>
    public class IMTClient
    {
        static MqttFactory factory = new MqttFactory();
        static IMqttClient client = factory.CreateMqttClient();
        static MqttClientOptions clientOptions = new MqttClientOptions();
        /// <summary>
        /// 状态
        /// </summary>
        public static class RunAsyncState
        {
            /// <summary>
            /// TCP服务是否开启
            /// </summary>
            public static Boolean state { get; set; } = false;
        }
        /// <summary>
        /// 返回的消息
        /// </summary>
        public class RunAsyncData
        {
            /// <summary>
            /// 返回状态
            /// </summary>
            public bool state { get; set; }
            /// <summary>
            /// 错误编码
            /// </summary>
            public bool errcode { get; set; }
            /// <summary>
            /// 消息头
            /// </summary>
            public String top { get; set; }
            /// <summary>
            /// 调用方法名称 消息中部
            /// </summary>
            public String Name { get; set; }
            /// <summary>
            /// mqttID 消息尾部
            /// </summary>
            public  String WithTopic { get; set; }
            /// <summary>
            /// 消息内容
            /// </summary>
            public String msg { get; set; }

        }
        /// <summary>
        /// 返回的消息
        /// </summary>
        public static class RunAsyncDataList
        {
            public static Dictionary<string, RunAsyncData> IMessage { get; set; } = new Dictionary<string, RunAsyncData>();

        }

        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <returns></returns>
        public static async Task RunAsync()
        {
            try
            {
                var logger = new MqttNetEventLogger();
                MqttNetConsoleLogger.ForwardToConsole(logger);


                clientOptions = new MqttClientOptions
                {
                    ChannelOptions = new MqttClientTcpOptions
                    {
                        Server = "127.0.0.1",
                        Port = 6667
                    },
                    ClientId = "IMTClient" + System.Guid.NewGuid().ToString("N"),
                    CleanSession = true,
                    Credentials = new MqttClientCredentials
                    {
                        Username = "aimap",
                        Password = Encoding.UTF8.GetBytes("ty1409ty.."),
                    }
                };
               

                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    
                    string datagram = (System.Text.Encoding.Default.GetBytes(Encoding.UTF8.GetString(e.ApplicationMessage.Payload)).Length > 4000 ? "字符串超长相关请查看LOG日志获取详细信息." : Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {datagram}");
                    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                    Console.WriteLine();
                    var a = e.ApplicationMessage.UserProperties;
                    //WEBAPI/GB28181/{Name}/{clientOptions.ClientId}
                    String url = $"{ e.ApplicationMessage.Topic.Split('/')[0] }/{ e.ApplicationMessage.Topic.Split('/')[1]}/{ e.ApplicationMessage.Topic.Split('/')[2]}/{ clientOptions.ClientId}";
                    if (IMTClient.RunAsyncDataList.IMessage.ContainsKey(url))
                    {
                        IMTClient.RunAsyncDataList.IMessage[url].msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    }

                    //if (RunAsyncData.WithTopic == e.ApplicationMessage.Topic) RunAsyncData.msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                });

                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                {
                    Console.WriteLine("### CONNECTED WITH SERVER ###");

                    await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("#").Build());
                    Console.WriteLine("### SUBSCRIBED ###");
                });

                client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
                {
                    Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    try
                    {
                        await client.ConnectAsync(clientOptions);
                        RunAsyncState.state = true;
                    }
                    catch
                    {
                        RunAsyncState.state = false;
                        Console.WriteLine("### RECONNECTING FAILED ###");
                    }
                });

                try
                {
                    await client.ConnectAsync(clientOptions);
                    RunAsyncState.state = true;
                }
                catch (Exception exception)
                {
                    RunAsyncState.state = false;
                    Console.WriteLine("### CONNECTING FAILED ###" + Environment.NewLine + exception);
                }

                Console.WriteLine("### WAITING FOR APPLICATION MESSAGES ###");
               
                while (true)
                {
                    Console.ReadLine();
                }


                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //static bool SendMessageState = false;
        /// <summary>
        /// 发送信息给服务端 System.Reflection.MethodBase.GetCurrentMethod().Name
        /// </summary>
        /// <returns></returns>
        public static IGB2818.IReturnParameters SendMessage(String Name,object obj,out int stateCode, bool await,bool cache) {
            stateCode = 200;
            if (!RunAsyncState.state) {

                stateCode = 501;
                return (new IGB2818.IReturnParameters
                {
                    //currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //declaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    msg = "与中间件无法连接或断开了连接.",
                    state = false,
                    more = { }
                }); 
            }
            try
            {
                String Json = JsonConvert.SerializeObject(obj);

               
                if (!IMTClient.RunAsyncDataList.IMessage.ContainsKey($"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}")) {

                    IMTClient.RunAsyncDataList.IMessage.Add($"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}", new RunAsyncData
                    {
                        msg = "",
                        Name = Name,
                        top = "WEBAPI/GB28181/",
                        WithTopic = clientOptions.ClientId
                    }) ;
                }
                if (cache && !String.IsNullOrEmpty(IMTClient.RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg))
                {
                    var IReturnParameters = (new IGB2818.IReturnParameters
                    {
                        //currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                        //declaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        data = JsonConvert.DeserializeObject<JObject>(RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg),
                        msg = "success",
                        state = true,
                        more = { }
                    });

                    string httpcode = null;
                    var data = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(IReturnParameters.data));
                    if (data.Property("stateCode") != null)
                    {
                        httpcode = data["stateCode"].ToString();
                    }
                    stateCode = Int32.Parse((string.IsNullOrEmpty(httpcode) ? "200" : httpcode));


                    return IReturnParameters;
                }

                if (await) 
                    IMTClient.RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg = null;
               
                //发布
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"WEBAPI/GB28181/{Name}/Client")
                    .WithPayload(Json)
                    .WithAtLeastOnceQoS()
                    .Build();
                client.PublishAsync(applicationMessage);
                //订阅
                client.SubscribeAsync(new MqttTopicFilter
                {
                    Topic = $"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}",
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce
                });
                Int32 count = 0;
                while (true)
                {
                    //处理抓取数据过快，返回自己
                    if (IMTClient.RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg == JsonConvert.SerializeObject(obj)) { IMTClient.RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg = null; }
                    if (!String.IsNullOrEmpty(RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg))
                    {
                        var IReturnParameters =(new IGB2818.IReturnParameters
                        {
                            //currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                            //declaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            data = JsonConvert.DeserializeObject<JObject>(RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg),
                            msg = "success",
                            state = true,
                            more = { }
                        });
                       
                        string httpcode = null;
                        var data = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(IReturnParameters.data));
                        if (data.Property("stateCode") != null) {
                            httpcode = data["stateCode"].ToString();
                        }
                        
                        stateCode = Int32.Parse((string.IsNullOrEmpty(httpcode) ? "200" : httpcode));


                        if (stateCode != 200)//防止按天缓冲的数据 中间报错无法刷新
                            IMTClient.RunAsyncDataList.IMessage[$"WEBAPI/GB28181/{Name}/{clientOptions.ClientId}"].msg = null;

                        return IReturnParameters;
                    }
                       
                    if (count >= (Int32)JsonConvert.DeserializeObject<JObject>(Json)["overtime"])
                    {
                        stateCode = 202;
                        return (new IGB2818.IReturnParameters
                        {
                            currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                            declaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            msg = "接口已超时...",
                            state = false,
                            more = { }
                        });
                    }
                    count++;
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                stateCode = 500;
                return (new IGB2818.IReturnParameters
                {
                    currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    declaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    msg = ex.Message,
                    state = false,
                    more = { }
                });
            }
            
        }
        /// <summary>
        /// 对回调进行约束
        /// </summary>
        public interface IGB2818
        {
            /// <summary>
            /// 回调声明
            /// </summary>
            /// <param name="RP"></param>
            public delegate JObject SomeCalculateWay(IReturnParameters RP);
            /// <summary>
            /// 回调的数据结构
            /// </summary>
            //返回
            public class IReturnParameters
            {
                
                /// <summary>
                /// 更多
                /// </summary>
                public object more { get; set; }
                /// <summary>
                /// 类名
                /// </summary>
                public String declaringType { get; set; }
                /// <summary>
                /// 方法名
                /// </summary>
                public String currentMethod { get; set; }
                /// <summary>
                /// 消息内容
                /// </summary>
                public object msg { get; set; }
                /// <summary>
                /// 消息内容
                /// </summary>
                public object data { get; set; }
                /// <summary>
                /// 状态
                /// </summary>
                public Boolean state { get; set; }
            }
            ///// <summary>
            ///// 回调的数据结构
            ///// </summary>
            ////返回
            //public class IReturnParameters
            //{
            //    /// <summary>
            //    /// 更多
            //    /// </summary>
            //    public object more { get; set; }
            //    /// <summary>
            //    /// 消息内容
            //    /// </summary>
            //    public object msg { get; set; }
            //    /// <summary>
            //    /// 消息内容
            //    /// </summary>
            //    public object data { get; set; }
            //    /// <summary>
            //    /// 状态
            //    /// </summary>
            //    public Boolean state { get; set; }
            //}

        }
    }
    public static class MqttNetConsoleLogger
    {
        static readonly object _lock = new object();

        public static void ForwardToConsole(MqttNetEventLogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            logger.LogMessagePublished -= PrintToConsole;
            logger.LogMessagePublished += PrintToConsole;
        }

        public static void PrintToConsole(string message, ConsoleColor color)
        {
            lock (_lock)
            {
                var backupColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(message);
                Console.ForegroundColor = backupColor;
            }
        }

        static void PrintToConsole(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            var output = new StringBuilder();
            output.AppendLine($">> [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}");
            if (e.LogMessage.Exception != null)
            {
                output.AppendLine(e.LogMessage.Exception.ToString());
            }

            var color = ConsoleColor.Red;
            switch (e.LogMessage.Level)
            {
                case MqttNetLogLevel.Error:
                    color = ConsoleColor.Red;
                    break;
                case MqttNetLogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case MqttNetLogLevel.Info:
                    color = ConsoleColor.Green;
                    break;
                case MqttNetLogLevel.Verbose:
                    color = ConsoleColor.Gray;
                    break;
            }

            PrintToConsole(output.ToString(), color);
        }
    }

}
