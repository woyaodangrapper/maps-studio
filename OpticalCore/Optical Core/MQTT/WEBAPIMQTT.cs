using ConsoleServer.IInterFace;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MQTTnet.Server.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpticalCore.MQTT
{
    public static class IQTServer
    {
        static IMqttServer mqttServer = new MqttFactory().CreateMqttServer();//mqtt服务端
        public class SMMRE{
            public  String key { get; set; }
            public  MqttApplicationMessageReceivedEventArgs MMRE { get; set; }
            //MqttApplicationMessageReceivedEventArgs
        }
        /// <summary>
        /// 缓存 需要回调的客户端(应对多并发)
        /// </summary>
        static List<SMMRE> L_action = new List<SMMRE>();
        public static void RunEmptyServer()
        {
            
            mqttServer.StartAsync(new MqttServerOptions()).GetAwaiter().GetResult();
            Console.WriteLine("Press any key to exit.");
            mqttServer.StopAsync();
        }
        /// <summary>
        /// 初始化MQSERVER
        /// </summary>
        /// <returns></returns>
        public static async Task RunAsync()
        {
            try
            {
                var logger = new MqttNetEventLogger();
                MqttNetConsoleLogger.ForwardToConsole(logger);
                //var options = new MqttServerOptions();
                //options.DefaultEndpointOptions.Port = int.Parse(ServerPort.Text);
                var options = new MqttServerOptions
                {
                  
                    ConnectionValidator = new MqttServerConnectionValidatorDelegate(p =>
                    {
                        if (p.ClientId == "OV_Mqtt")
                        {
                            if (p.Username != "aimap" || p.Password != "OV_Mqtt..")
                            {
                                p.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            }
                        }
                        
                    }),
                   
                    Storage = new RetainedMessageHandler(),

                    ApplicationMessageInterceptor = new MqttServerApplicationMessageInterceptorDelegate(context =>
                    {
                        if (MqttTopicFilterComparer.IsMatch(context.ApplicationMessage.Topic, "/myTopic/WithTimestamp/#"))
                        {
                            // Replace the payload with the timestamp. But also extending a JSON 
                            // based payload with the timestamp is a suitable use case.
                            context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(DateTime.Now.ToString("O"));
                        }

                        if (context.ApplicationMessage.Topic == "not_allowed_topic")
                        {
                            context.AcceptPublish = false;
                            context.CloseConnection = true;
                        }
                    }),

                    SubscriptionInterceptor = new MqttServerSubscriptionInterceptorDelegate(context =>
                    {
                        if (context.TopicFilter.Topic.StartsWith("admin/foo/bar") && context.ClientId != "theAdmin")
                        {
                            context.AcceptSubscription = false;
                        }

                        if (context.TopicFilter.Topic.StartsWith("the/secret/stuff") && context.ClientId != "Imperator")
                        {
                            context.AcceptSubscription = false;
                            context.CloseConnection = true;
                        }
                    })
                };
                options.DefaultEndpointOptions.Port = 6667;
                // Extend the timestamp for all messages from clients.
                // Protect several topics from being subscribed from every client.

                //var certificate = new X509Certificate(@"C:\certs\test\test.cer", "");
                //options.TlsEndpointOptions.Certificate = certificate.Export(X509ContentType.Cert);
                //options.ConnectionBacklog = 5;
                //options.DefaultEndpointOptions.IsEnabled = true;
                //options.TlsEndpointOptions.IsEnabled = false;

                mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {

                    //Log.Debug($"Send : {(System.Text.Encoding.Default.GetBytes(datagram).Length > 4000 ? "字符串超长相关请查看LOG日志获取详细信息." : datagram)}");//TCP终端服务器开启成功.

                    string datagram = $"'{e.ClientId}' reported '{e.ApplicationMessage.Topic}' > '{Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0])}'" + System.Environment.NewLine;
                    MqttNetConsoleLogger.PrintToConsole(
                        (System.Text.Encoding.Default.GetBytes(datagram).Length > 4000 ? "字符串超长相关请查看LOG日志获取详细信息." : datagram),
                        ConsoleColor.Magenta
                    );
                    if (String.IsNullOrEmpty(e.ClientId)) return;
                    if (e.ClientId.Contains("IMTClient")) {
                        var RP = ObjectRequestProcessing(Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]),out bool cb);
                        if (!cb)
                        { //如果不包含回调
                            var applicationMessage = new MqttApplicationMessageBuilder()
                              .WithTopic(e.ApplicationMessage.Topic.Replace("Client", e.ClientId))
                              .WithPayload(RP)
                              .WithAtLeastOnceQoS()
                              .Build();
                            mqttServer.PublishAsync(applicationMessage);
                        }//缓存 需要回调的客户端
                        else {
                            L_action.Add(new SMMRE() {
                                key = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]),
                                MMRE = e
                            });
                            //ObjectRequestProcessing(Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]), out _);

                        }

                    }
                });

                //options.ApplicationMessageInterceptor = c =>
                //{
                //    if (c.ApplicationMessage.Payload == null || c.ApplicationMessage.Payload.Length == 0)
                //    {
                //        return;
                //    }

                //    try
                //    {
                //        var content = JObject.Parse(Encoding.UTF8.GetString(c.ApplicationMessage.Payload));
                //        var timestampProperty = content.Property("timestamp");
                //        if (timestampProperty != null && timestampProperty.Value.Type == JTokenType.Null)
                //        {
                //            timestampProperty.Value = DateTime.Now.ToString("O");
                //            c.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(content.ToString());
                //        }
                //    }
                //    catch (Exception)
                //    {
                //    }
                //};
                var CurrentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
                var DeclaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
                {
                       
                    Log.Debug($"Client disconnected event fired.{CurrentMethod},{DeclaringType}");

                });

                await mqttServer.StartAsync(options);
                Log.Debug($"mqttServer 启动完成{CurrentMethod},{DeclaringType}");
                  
                //await mqttServer.StopAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

        }
      
        /// <summary>
        /// 对发送对象做数据处理 并且返回
        /// </summary>
        /// <param name="rb">json</param>
        /// <param name="callback">是否需要判断回调</param>
        /// <returns></returns>
        public static String ObjectRequestProcessing(String msg,out bool callback)
        {
            callback = false;
            if (String.IsNullOrEmpty(msg)) return JsonConvert.SerializeObject(new
            {
                ObjectRequestProcessing = "请勿将固定参数留空!"
            });

            var rb = JsonConvert.DeserializeObject<JObject>(msg);
            #region 接口处理
            bool methodExistence; object Data;
            methodExistence = false; Data = null;
            try
            {
                foreach (var item in Program.Method.Keys)
                {
                    var mod = Program.Method[item];
                    MethodInfo method = mod.type.GetMethod(rb["eventType"].ToString());
                    if (method != null)
                    {
                        methodExistence = true;
                        object[] obj =
                        {
                            rb
                        };
                        if (method.GetParameters().Length > 0)
                            //对方法进行调用
                            Data = method.Invoke(mod.o, obj);//param为方法参数object数组
                        else Data = method.Invoke(mod.o, null);
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    stateCode = 500,
                    ObjectRequestProcessing = ex.Message
                });

            }
            if (!methodExistence)
            {
                return JsonConvert.SerializeObject(new
                {
                    stateCode = 404,
                    ObjectRequestProcessing = $"未查询到当前,对接{rb["eventType"]}对应接口！"
                });
            }
            else
            {
                return JsonConvert.SerializeObject(Data);
            }
            #endregion
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
    public class RetainedMessageHandler : IMqttServerStorage
    {
        private const string Filename = "C:\\MQTT\\RetainedMessages.json";

        public Task SaveRetainedMessagesAsync(IList<MqttApplicationMessage> messages)
        {
            var directory = Path.GetDirectoryName(Filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(Filename, JsonConvert.SerializeObject(messages));
            return Task.FromResult(0);
        }

        public Task<IList<MqttApplicationMessage>> LoadRetainedMessagesAsync()
        {
            IList<MqttApplicationMessage> retainedMessages;
            if (File.Exists(Filename))
            {
                var json = File.ReadAllText(Filename);
                retainedMessages = JsonConvert.DeserializeObject<List<MqttApplicationMessage>>(json);
            }
            else
            {
                retainedMessages = new List<MqttApplicationMessage>();
            }

            return Task.FromResult(retainedMessages);
        }
    }

}
