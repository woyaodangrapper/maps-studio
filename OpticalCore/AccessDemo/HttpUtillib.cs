using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static HttpUtil.HttpUtillib;

namespace HttpUtil
{
    public class HttpUtillib
    {
        public static void WriteLog(String Name, String strLog)
        {
            String sFilePath = DateTime.Now.ToString("yyyyMM");
            String sFileName = Name + DateTime.Now.ToString("dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 设置信息参数
        /// </summary>
        /// <param name="appkey">合作方APPKey</param>
        /// <param name="secret">合作方APPSecret</param>
        /// <param name="ip">平台IP</param>
        /// <param name="port">平台端口，默认HTTPS的443端口</param>
        /// <param name="isHttps">是否启用HTTPS协议，默认HTTPS</param>
        /// <return></return>
        public static void SetPlatformInfo(string appkey, string secret, string ip, int port = 443, bool isHttps = true, string type = "ISC/SPCC")
        {
            keyModels.Add(new KeyModel
            {
                type = type,
                _appkey = appkey,
                _secret = secret,
                _ip = ip,
                _port = port,
                _isHttps = isHttps
            });
            // 设置并发数，如不设置默认为2
            ServicePointManager.DefaultConnectionLimit = 512;
        }

        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="uri">HTTP接口Url，不带协议和端口，如/artemis/api/resource/v1/cameras/indexCode?cameraIndexCode=a10cafaa777c49a5af92c165c95970e0</param>
        /// <param name="timeout">请求超时时间，单位：秒</param>
        /// <returns></returns>
        public static byte[] HttpGet(string uri, int timeout, string type = "ISC/SPCC")
        {
            #region 重新初始化秘钥
            {
                var km = keyModels.GetKey(type);
                _ip = km._ip;
                _port = km._port;
                _appkey = km._appkey;
                _secret = km._secret;
                _isHttps = km._isHttps;
            }
            #endregion

            Dictionary<string, string> header = new Dictionary<string, string>();

            // 初始化请求：组装请求头，设置远程证书自动验证通过
            initRequest(header, uri, "", false);

            // build web request object
            StringBuilder sb = new StringBuilder();
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);
            Log.Debug(sb.ToString());
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version11;
            req.AllowAutoRedirect = false;   // 不允许自动重定向
            req.Method = "GET";
            req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
            req.Accept = header["Accept"];
            req.ContentType = header["Content-Type"];

            foreach (string headerKey in header.Keys)
            {
                if (headerKey.Contains("x-ca-"))
                {
                    req.Headers.Add(headerKey + ":" + header[headerKey]);
                }
            }

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                if (HttpStatusCode.OK == rsp.StatusCode)
                {
                    Stream rspStream = rsp.GetResponseStream();     // 响应内容字节流
                    StreamReader sr = new StreamReader(rspStream);
                    string strStream = sr.ReadToEnd();
                    long streamLength = strStream.Length;
                    byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
                    rsp.Close();
                    return response;
                }
                else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
                {
                    string reqUrl = rsp.Headers["Location"].ToString();   // 获取重定向URL
                    WebRequest wreq = WebRequest.Create(reqUrl);          // 重定向请求对象
                    WebResponse wrsp = wreq.GetResponse();                // 重定向响应
                    long streamLength = wrsp.ContentLength;               // 重定向响应内容长度
                    Stream rspStream = wrsp.GetResponseStream();          // 响应内容字节流
                    byte[] response = new byte[streamLength];
                    rspStream.Read(response, 0, (int)streamLength);       // 读取响应内容至byte数组
                    rspStream.Close();
                    rsp.Close();
                    return response;
                }

                rsp.Close();
            }
            catch (WebException e)
            {
                if (rsp != null)
                {
                    rsp.Close();
                }
            }

            return null;
        }

        /// <summary>
        /// HTTP Post请求
        /// </summary>
        /// <param name="uri">HTTP接口Url，不带协议和端口，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="body">请求参数</param>
        /// <param name="timeout">请求超时时间，单位：秒</param>
        /// <return>请求结果</return>
        public static byte[] HttpPost(string uri, string body, int timeout, string type = "ISC/SPCC")
        {
            #region 重新初始化秘钥
            {
                var km = keyModels.GetKey(type);
                _ip = km._ip;
                _port = km._port;
                _appkey = km._appkey;
                _secret = km._secret;
                _isHttps = km._isHttps;
            }
            #endregion

            static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
            Dictionary<string, string> header = new Dictionary<string, string>();

            // 初始化请求：组装请求头，设置远程证书自动验证通过
            initRequest(header, uri, body, true);

            // build web request object
            StringBuilder sb = new StringBuilder();
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);
            Log.Debug(sb.ToString());
            // 创建POST请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());


            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //声明协议
            // 准备请求...
            // 设置参数
            if (sb.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //处理Https请求
                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(CheckValidationResult);
                req = WebRequest.Create(sb.ToString()) as HttpWebRequest;
                req.ProtocolVersion = HttpVersion.Version10;

            }
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version11;
            req.AllowAutoRedirect = false;   // 不允许自动重定向
            req.Method = "POST";
            req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
            req.Accept = header["Accept"];
            req.ContentType = header["Content-Type"];
            req.UserAgent = "PostmanRuntime/7.26.8";

            foreach (string headerKey in header.Keys)
            {
                if (headerKey.Contains("x-ca-"))
                {
                    req.Headers.Add(headerKey + ":" + header[headerKey]);

                }
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(body);
                req.ContentLength = postBytes.Length;
                Stream reqStream = null;

                try
                {
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postBytes, 0, postBytes.Length);
                    reqStream.Close();
                }
                catch (WebException e)
                {
                    if (reqStream != null)
                    {
                        reqStream.Close();
                    }

                    return null;
                }
            }

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                if (HttpStatusCode.OK == rsp.StatusCode)
                {
                    Stream rspStream = rsp.GetResponseStream();
                    StreamReader sr = new StreamReader(rspStream);
                    string strStream = sr.ReadToEnd();
                    long streamLength = strStream.Length;
                    byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
                    rsp.Close();
                    return response;
                }
                else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
                {
                    try
                    {
                        string reqUrl = rsp.Headers["Location"].ToString();    // 如需要重定向URL，请自行修改接口返回此参数
                        rsp.Close();
                        WebRequest wreq = WebRequest.Create(reqUrl);
                        rsp = (HttpWebResponse)wreq.GetResponse();
                        Stream rspStream = rsp.GetResponseStream();
                        long streamLength = rsp.ContentLength;
                        int offset = 0;
                        byte[] response = new byte[streamLength];
                        while (streamLength > 0)
                        {
                            int n = rspStream.Read(response, offset, (int)streamLength);
                            if (0 == n)
                            {
                                break;
                            }

                            offset += n;
                            streamLength -= n;
                        }

                        return response;
                    }
                    catch (Exception e)
                    {
                        rsp.Close();
                        Log.Error(e.Message);
                        return null;
                    }
                }

                rsp.Close();
            }
            catch (WebException e)
            {
                if (rsp != null)
                {
                    rsp.Close();
                }
                Log.Error(e.Message);
            }

            return null;
        }
        public static byte[] HIK_HTTP(string uri, string body, string User_Agent, string type = "ISC/SPCC", bool get = false)
        {

            #region 重新初始化秘钥
            {
                var km = keyModels.GetKey(type);
                _ip = km._ip;
                _port = km._port;
                _appkey = km._appkey;
                _secret = km._secret;
                _isHttps = km._isHttps;
            }
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);
            Log.Debug(sb.ToString());

            var Date = DateTime.Now.ToString("R");
            /// <summary>
            /// HMACSHA256 =》 Base64
            /// </summary>
            /// <param name="message">内容</param>
            /// <param name="secret">秘钥</param>
            /// <returns></returns>
            string CreateToken(string message, string secret)
            {
                secret = secret ?? "";
                var encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);
                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    return Convert.ToBase64String(hashmessage);
                }
            }
            /// <summary>
            /// 用MD5加密字符串
            /// </summary>
            /// <param name="password">待加密的字符串</param>
            /// <returns></returns>
            string MD5Encrypt(string password)
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedDataBytes;
                hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder tmp = new StringBuilder();
                foreach (byte i in hashedDataBytes)
                {
                    tmp.Append(i.ToString("x2"));
                }
                return tmp.ToString();
            }

            long Time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            //AppToken = time + MD5(请求方法 + 请求URI + time + AppSecurity)；
            //String Host = $"{(get ? "GET" : "POST")}\napplication/json\napplication/json\n{Date}\nx-ca-key:{_appkey}\n{""}\n{uri}";
            //if (type != "PICSAPI")
            //{
            //    Host = $"{(get ? "GET" : "POST")}\napplication/json\napplication/json\n{Date}\nx-ca-key:{_appkey}\n{uri}";
            //}



            var time = new DateTime((Convert.ToInt64(Time) * 10000) + 621355968000000000);

            String FOMKey = Time.ToString() + MD5Encrypt((get ? "GET" : "POST") + uri.Split("?")[0] + Time.ToString() + _secret);
            {
                Log.Debug("Time:" + Time);
                Log.Debug("MD5:" + MD5Encrypt((get ? "GET" : "POST") + uri + Time.ToString() + _secret));
                Log.Debug("MD5Data:" + (get ? "GET" : "POST") + uri + Time.ToString() + _secret);
                Log.Debug("AppKey:" + _appkey);
            }

            var client = new RestClient(sb.ToString())
            {
                Timeout = -1
            };
            var request = new RestRequest(get ? Method.GET : Method.POST);
            request.AddHeader("App-Key", _appkey);
            request.AddHeader("App-Token", FOMKey);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            if (sb.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                client.RemoteCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            }

            IRestResponse response = client.Execute(request);
            return Encoding.UTF8.GetBytes(response.Content);
        }
        private static void initRequest(Dictionary<string, string> header, string url, string body, bool isPost)
        {
            // Accept                
            string accept = "application/json";// "*/*";
            header.Add("Accept", accept);

            // ContentType  
            string contentType = "application/json";
            header.Add("Content-Type", contentType);

            if (isPost)
            {
                // content-md5，be careful it must be lower case.
                string contentMd5 = computeContentMd5(body);
                header.Add("content-md5", contentMd5);
            }

            // x-ca-timestamp
            string timestamp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
            header.Add("x-ca-timestamp", timestamp);

            // x-ca-nonce
            string nonce = System.Guid.NewGuid().ToString();
            header.Add("x-ca-nonce", nonce);

            // x-ca-key
            header.Add("x-ca-key", _appkey);

            // build string to sign
            string strToSign = buildSignString(isPost ? "POST" : "GET", url, header);
            string signedStr = computeForHMACSHA256(strToSign, _secret);

            // x-ca-signature
            header.Add("x-ca-signature", signedStr);

            //if (_isHttps)
            //{
            //    // set remote certificate Validation auto pass
            //    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidate);
            //    // FIX：修复不同.Net版对一些SecurityProtocolType枚举支持情况不一致导致编译失败等问题，这里统一使用数值
            //    //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)3072 | (SecurityProtocolType)768 | (SecurityProtocolType)192;
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //}
        }

        /// <summary>
        /// 计算content-md5
        /// </summary>
        /// <param name="body"></param>
        /// <returns>base64后的content-md5</returns>
        private static string computeContentMd5(string body)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(body));
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 计算HMACSHA265
        /// </summary>
        /// <param name="str">待计算字符串</param>
        /// <param name="secret">平台APPSecet</param>
        /// <returns>HMAXH265计算结果字符串</returns>
        private static string computeForHMACSHA256(string str, string secret)
        {
            var encoder = new System.Text.UTF8Encoding();
            byte[] secretBytes = encoder.GetBytes(secret);
            byte[] strBytes = encoder.GetBytes(str);
            var opertor = new HMACSHA256(secretBytes);
            byte[] hashbytes = opertor.ComputeHash(strBytes);
            return Convert.ToBase64String(hashbytes);
        }

        /// <summary>
        /// 计算签名字符串
        /// </summary>
        /// <param name="method">HTTP请求方法，如“POST”</param>
        /// <param name="url">接口Url，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="header">请求头</param>
        /// <returns>签名字符串</returns>
        private static string buildSignString(string method, string url, Dictionary<string, string> header)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ToUpper()).Append("\n");
            if (null != header)
            {
                if (null != header["Accept"])
                {
                    sb.Append((string)header["Accept"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Content-MD5") && null != header["Content-MD5"])
                {
                    sb.Append((string)header["Content-MD5"]);
                    sb.Append("\n");
                }

                if (null != header["Content-Type"])
                {
                    sb.Append((string)header["Content-Type"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Date") && null != header["Date"])
                {
                    sb.Append((string)header["Date"]);
                    sb.Append("\n");
                }
            }

            // build and add header to sign
            string signHeader = buildSignHeader(header);
            sb.Append(signHeader);
            sb.Append(url);
            return sb.ToString();
        }

        /// <summary>
        /// 计算签名头
        /// </summary>
        /// <param name="header">请求头</param>
        /// <returns>签名头</returns>
        private static string buildSignHeader(Dictionary<string, string> header)
        {
            Dictionary<string, string> sortedDicHeader = new Dictionary<string, string>();
            sortedDicHeader = header;
            var dic = from objDic in sortedDicHeader orderby objDic.Key ascending select objDic;

            StringBuilder sbSignHeader = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                if (kvp.Key.Replace(" ", "").Contains("x-ca-"))
                {
                    sb.Append(kvp.Key + ":");
                    if (!string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        sb.Append(kvp.Value);
                    }
                    sb.Append("\n");
                    if (sbSignHeader.Length > 0)
                    {
                        sbSignHeader.Append(",");
                    }
                    sbSignHeader.Append(kvp.Key);
                }
            }

            header.Add("x-ca-signature-headers", sbSignHeader.ToString());

            return sb.ToString();
        }

        public class KeyModel
        {
            public string type { get; set; }
            /// <summary>
            /// 平台ip
            /// </summary>
            public string _ip { get; set; }

            /// <summary>
            /// 平台端口
            /// </summary>
            public int _port { get; set; } = 443;

            /// <summary>
            /// 平台APPKey
            /// </summary>
            public string _appkey { get; set; }

            /// <summary>
            /// 平台APPSecret
            /// </summary>
            public string _secret { get; set; }

            /// <summary>
            /// 是否使用HTTPS协议
            /// </summary>
            public bool _isHttps { get; set; } = true;

        }
        public static List<KeyModel> keyModels = new List<KeyModel>();

        /// <summary>
        /// 平台ip
        /// </summary>
        private static string _ip;

        /// <summary>
        /// 平台端口
        /// </summary>
        private static int _port = 443;

        /// <summary>
        /// 平台APPKey
        /// </summary>
        private static string _appkey;

        /// <summary>
        /// 平台APPSecret
        /// </summary>
        private static string _secret;

        /// <summary>
        /// 是否使用HTTPS协议
        /// </summary>
        private static bool _isHttps = true;
    }
}

namespace HttpUtil
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class Iexpand
    {
        /// <summary>
        /// 拓展obj 取签名
        /// </summary>
        /// <param name="obj">签名集合</param>
        /// <param name="Name">方法名称</param>
        /// <returns></returns>
        public static KeyModel GetKey(this Object obj, String Name)
        {
            KeyModel km = null;
            foreach (var item in keyModels)
            {
                if (item.type == Name)
                    km = item;
            }
            return km;
        }

    }
}
