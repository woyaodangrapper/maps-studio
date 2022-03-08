using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;

namespace Optical_View.Class
{
    /// <summary>
    /// 互联网请求类
    /// </summary>
    public static class AjaxCS
    {
        public static JObject Post(string url, string body)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Log.Debug(" Optical_View.Class.HTTP.Post" + response.Content + " body:" + body + "url:" + url);
            if (response.Content == null) { response.Content = "{}"; }
            return JsonConvert.DeserializeObject<JObject>(response.Content);
        }
    }
}
