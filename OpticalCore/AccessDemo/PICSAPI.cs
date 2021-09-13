using HttpUtil;
using InterFace;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace PICSAPI
{
    public class PICSAPI : IExpand
    {
        public void Init(object[] obj)
        {
            #region 初始化LOG日志
            
            //{
            //    Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.Console()
            //    .MinimumLevel.Debug()
            //    .WriteTo.File(Path.Combine("PICSAPI", DateTime.Now.ToString("yyyyMMdd") + $".txt"),
            //        rollingInterval: RollingInterval.Day,
            //        rollOnFileSizeLimit: true)
            //    .CreateLogger();
            //    Log.Debug("init");
            //}
            #endregion
            HttpUtillib.SetPlatformInfo("99841828008864760000", "8444101804936925000", "192.168.100.10", 443,true, "PICSAPI");
        }
        #region PICSAPI接口
       
        public static String _Iexpand(String url, out bool state, String json = null, String User_Agent = "PostmanRuntime/7.26.8", string type = "PICSAPI", bool get = false)
        {
            // 组装POST请求body      
            string body = json;
            // 发起POST请求，超时时间15秒，返回响应字节数组
            try
            {
                Log.Debug("url:" + url);
                Log.Debug("json:" + json);
                string uri = "/" + url;
                byte[] result = HttpUtillib.HIK_HTTP(uri, body, User_Agent, type, get);
                string msg = System.Text.Encoding.UTF8.GetString(result == null ? new byte[] { 0 } : result);
                if (System.Text.Encoding.Default.GetBytes(msg).Length > 4000)
                {
                    HttpUtillib.WriteLog("超长字符串", msg);
                }
                string datagram = (System.Text.Encoding.Default.GetBytes(msg).Length > 4000 ? "字符串超长相关请查看LOG日志获取详细信息." : msg);
                Log.Debug(datagram);
                //string datagram = (System.Text.Encoding.Default.GetBytes(Encoding.UTF8.GetString(e.ApplicationMessage.Payload)).Length > 4000 ? "字符串超长相关请查看LOG日志获取详细信息." : Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                if (string.IsNullOrEmpty(msg))
                {
                    state = false;
                    // 请求失败
                    return null;
                }
                else
                {
                    state = true;
                    return System.Text.Encoding.UTF8.GetString(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
                state = false;
                // 请求失败
                return null;
            }

        }
       
        /// <summary>
        /// 以图搜图
        /// </summary>
        public static object SFPWP(JObject rb)
        {
            // + 28800000
          


            string cameracode = null;
            string parameter = _Iexpand("intelligence/judicial/v1/person/outcomer/register", out bool state, JsonConvert.SerializeObject(new
            {
                cameraIndexCode = cameracode,
            }), "PostmanRuntime/7.26.8", "PICSAPI", false);


            

            if (state)
            {
                JObject rbs = JsonConvert.DeserializeObject<JObject>(parameter);
                return rbs;
            }
            else
                return (new
                {
                    stateCode = 502,
                    msg="无法连接海康服务器！"
                });

           
        }


        class Iv
        {
            public JToken type { get; set; }
            public JToken list { get; set; }
        }
        /// <summary>
        /// 获取人员名单
        /// </summary>
        public static object VisitorList(JObject rb)
        {
            // + 28800000
            string parameter = _Iexpand("intelligence/judicial/v1/listlibrary", out bool state, null, "PostmanRuntime/7.26.8", "PICSAPI", false);

            var list = new
            {
                visitorList = new List<Iv>()
            };

            if (state)
            {
                JObject rbs = JsonConvert.DeserializeObject<JObject>(parameter);

                foreach (var item in rbs["data"])
                {

                    //intelligence/judicial/v1/namelist?listlibraryId=1&pageNo=1&pageSize=1000
                    var Msg = _Iexpand($"intelligence/judicial/v1/namelist?listlibraryId={item["id"]}&pageNo=1&pageSize=1000", out  state, null, "PostmanRuntime/7.26.8", "PICSAPI", false);
                    if (state)
                    {
                        list.visitorList.Add(new Iv
                        {
                            type = item,
                            list = JsonConvert.DeserializeObject<JObject>(Msg),
                        });
                    }
                    else
                    {
                        return (new
                        {
                            VisitorList = "查询被中断。"
                        });
                    }
                }

                return list;
            }
            else
                return (new
                {
                    stateCode = 502,
                    msg = "无法连接海康服务器！"
                });

        }

        /// <summary>
        /// 获取人员名单
        /// </summary>
        public static object TodayVisitors(JObject rb)
        {
            // + 28800000
            string parameter = _Iexpand("intelligence/judicial/v1/listlibrary", out bool state, null, "PostmanRuntime/7.26.8", "PICSAPI", false);

            var list = new
            {
                visitorList = new List<Iv>()
            };
            if (state)
            {
                JObject rbs = JsonConvert.DeserializeObject<JObject>(parameter);

                foreach (var item in rbs["data"])
                {
                    if (item["name"].ToString() == "今日来访人员库") {
                        //intelligence/judicial/v1/namelist?listlibraryId=1&pageNo=1&pageSize=1000
                        var Msg = _Iexpand($"intelligence/judicial/v1/namelist?listlibraryId={item["id"]}&pageNo=1&pageSize=1000", out state, null, "PostmanRuntime/7.26.8", "PICSAPI", false);
                        if (state)
                        {
                            list.visitorList.Add(new Iv
                            {
                                type = item,
                                list = JsonConvert.DeserializeObject<JObject>(Msg)["data"]["list"],
                            });
                        }
                        else
                            return (new
                            {
                                VisitorList = "查询被中断。"
                            });
                    }
                   
                }

                return list;
            }
            else
                return (new
                {
                    stateCode = 502,
                    msg = "无法连接海康服务器！"
                });

        }
        /// <summary>
        /// 查询来访记录
        /// </summary>
        public static object IAOR(JObject rb)
        {
            // + 28800000
            string parameter = _Iexpand("intelligence/judicial/v1/event/visit", out bool state, JsonConvert.SerializeObject(new
            {
                pageNo = 1,
                pageSize = 10
            }), "PostmanRuntime/7.26.8", "PICSAPI", false);
          
            if (state)
            {
                JObject rbs = JsonConvert.DeserializeObject<JObject>(parameter);

                return rbs;
            }
            else
                return (new
                {
                    stateCode = 502,
                    msg = "无法连接海康服务器！"
                });

        }


        #endregion

    }

}
