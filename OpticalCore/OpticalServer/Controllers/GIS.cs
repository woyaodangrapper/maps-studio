using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalServer.Controllers
{
    /// <summary>
    /// 文件夹配置
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("GIS/Folder")]
    public class GISFolder : Controller
    {

        Int32 overtime = 20;//回调超时时间

        /// <summary>
        /// 获取数据模型接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GDM")]//camera
        public JObject GetDataModel(OpticalServer.Model.Object.GetDataModel json)
        {
            String Name = System.Reflection.MethodBase.GetCurrentMethod().Name;//事件名称代码
            var msg = new
            {
                MsgType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                eventType = Name,
                overtime, //超时时间
                json
            }.Sends(Name, Response, null, false, false);
            return msg;

        }
       
    }
}
