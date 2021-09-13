using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpticalServer.MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalServer
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class Iexpand
    {
        /// <summary>
        /// 拓展obj mqtt发生等待返回数据
        /// </summary>
        /// <param name="obj">发送的对象</param>
        /// <param name="Name">方法名称</param>
        /// <param name="Response">返回接口状态码</param>
        /// <param name="await">是否等待返回(数据获取加速1)</param>
        /// <param name="cache">是否缓存数据(数据获取加速2 必须 1 为 false)</param>
        /// <returns></returns>
        public static JObject Sends(this Object obj, String Name,  HttpResponse Response,string more = null,bool await = false,bool cache = false)
        {
            var msg = IMTClient.SendMessage(Name, obj, out int stateCode, await, cache);
            msg.more = more;
            if (cache)  msg.more += "(当前接口缓存为开启,接口数据一天同步一次)"; 
            Response.StatusCode = stateCode;
            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(msg));
        }

    }
}
