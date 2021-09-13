using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Optical_View.Class
{
    public class Windows_ImWebServer
    {
        public  bool running = false; // Is it running?

        private  int timeout = 8; // Time limit for data transfers.
        private  Encoding charEncoder = Encoding.UTF8; // To encode string
        private  Socket serverSocket; // Our server socket
        private static string contentPath; // Root path of our contents

        // Content types that are supported by our server
        // You can add more...
        // To see other types: 
        private  Dictionary<string, string> extensions = new Dictionary<string, string>()
        { 
            //{ "extension", "content type" }
            { "htm", "text/html" },
            { "b3dm", "application/zip" },
            { "svg", "image/svg+xml" },
            { "cmpt", "application/zip" },
            { "js", "application/javascript" },
            { "json", "application/json" },
            { "html", "text/html" },
            { "xml", "text/xml" },
            { "txt", "text/plain" },
            { "css", "text/css" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "jpg", "image/jpg" },
            { "jpeg", "image/jpeg" },
            { "zip", "application/zip"}
        };
        public  bool start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false; // If it is already running, exit.

            try
            {
                // A tcp/ip socket (ipv4)
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
                Windows_ImWebServer.contentPath = contentPath;//contentPath;
            }
            catch (System.Exception e) { 
                System.Console.WriteLine($"start webserver error({e.Message})");  
                return false; 
            }

            // Our thread that will listen connection requests
            // and create new threads to handle them.
            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        // Create new thread to handle the request and continue to listen the socket.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { handleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { System.Console.WriteLine("requestHandler 异常退出"); }
                            }
                        });
                        requestHandler.Name = "监听服务器";
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Name = "三维 代理服务器";
            requestListenerT.Start();
            return true;
        }
        public  void stop()
        {
            if (running)
            {
                running = false;
                try { serverSocket.Close(); }
                catch(System.Exception e) { System.Console.WriteLine("serverSocket 关闭失败-" + e.Message); }
                serverSocket = null;
            }
        }
        private  void handleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);

            string requestedFile;
            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else // You can implement other methods...
            {
                notImplemented(clientSocket);
                return;
            }

            string uri = "/";
            for (int i = 0; i < requestedFile.Split("/").Length; i++)
            {
                var itm = requestedFile.Split("/")[i];
                itm = System.Web.HttpUtility.UrlDecode(itm);
                if (!System.String.IsNullOrEmpty(itm))
                {
                    uri += itm + (i < requestedFile.Split("/").Length - 1 ? "/" : "");
                }
            }
            requestedFile = uri;

            requestedFile = requestedFile.Replace("/", @"\").Replace("\\..", "");
            Log.Debug("requestedFile:" + requestedFile);

            start = requestedFile.LastIndexOf('.') + 1;
            if (start > 0)
            {
                length = requestedFile.Length - start;
                string extension = requestedFile.Substring(start, length);
                if (extensions.ContainsKey(extension)) // Do we support this extension?
                    if (File.Exists(contentPath + requestedFile)) //If yes check existence of the file
                                                                  // Everything is OK, send requested file with correct content type:
                        sendOkResponse(clientSocket,
                          File.ReadAllBytes(contentPath + requestedFile), extensions[extension]);
                    else
                        notFound(clientSocket); // We don't support this extension.
                                                // We are assuming that it doesn't exist.
            }
            else
            {
                // If file is not specified try to send index.htm or index.html
                // You can add more (default.htm, default.html)
                if (requestedFile.Substring(length - 1, 1) != @"\")
                    requestedFile += @"\";
                if (File.Exists(contentPath + requestedFile + "index.htm"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.htm"), "text/html");
                else if (File.Exists(contentPath + requestedFile + "index.html"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.html"), "text/html");
                else
                    notFound(clientSocket);
            }
          

        }
        private  void notImplemented(Socket clientSocket)
        {

            sendResponse(clientSocket, "<html><head><meta " +
             "http -equiv=\"Content-Type\" content=\"text/html; " +
             "charset =utf-8\">" +
             "</head><body><h2>Atasoy Simple Web " +
             "Server </h2><div>501 - Method Not " +
             "Implemented </div></body></html>",
             "501 Not Implemented", "text/html");

        }

        private  void notFound(Socket clientSocket)
        {

            sendResponse(clientSocket, "<html><head><meta " +
             "http-equiv=\"Content-Type\" content=\"text/html; " +
             "charset =utf-8\"></head><body><h2>Atasoy Simple Web " +
             "Server </h2><div>404 - Not " +
             "Found </div></body></html>",
             "404 Not Found", "text/html");
        }

        private  void sendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            sendResponse(clientSocket, bContent, "200 OK", contentType);
        }

        // For strings
        private  void sendResponse(Socket clientSocket, string strContent, string responseCode,
                                  string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            sendResponse(clientSocket, bContent, responseCode, contentType);
        }

        // For byte arrays
        private  void sendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                                  string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Atasoy Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"

                                  + "Access-Control-Allow-Methods: OPTIONS,POST,GET\r\n"
                                  + "Access-Control-Allow-Headers: x-requested-with,content-type\r\n"
                                  + "Access-Control-Allow-Origin: *\r\n"


                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch { }
        }
    }
    public  class Linux_ImWebServer
    {
        public  bool running = false; // Is it running?

        private  int timeout = 8; // Time limit for data transfers.
        private  Encoding charEncoder = Encoding.UTF8; // To encode string
        private  Socket serverSocket; // Our server socket
        private static string contentPath; // Root path of our contents

        // Content types that are supported by our server
        // You can add more...
        // To see other types: 
        private  Dictionary<string, string> extensions = new Dictionary<string, string>()
        { 
            //{ "extension", "content type" }
            { "htm", "text/html" },
            { "b3dm", "application/zip" },
            { "cmpt", "application/zip" },
            { "js", "application/javascript" },
            { "json", "application/json" },
            { "html", "text/html" },
            { "xml", "text/xml" },
            { "txt", "text/plain" },
            { "css", "text/css" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "jpg", "image/jpg" },
            { "jpeg", "image/jpeg" },
            { "zip", "application/zip"}
        };
        public  bool start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false; // If it is already running, exit.

            try
            {
                // A tcp/ip socket (ipv4)
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
                Linux_ImWebServer.contentPath = contentPath;//contentPath;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"start webserver error({e.Message})");
                return false;
            }

            // Our thread that will listen connection requests
            // and create new threads to handle them.
            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        // Create new thread to handle the request and continue to listen the socket.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { handleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { System.Console.WriteLine("requestHandler 异常"); }
                            }
                        });
                        requestHandler.Name = "监听服务器";
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Name = "三维 代理服务器";
            requestListenerT.Start();
            return true;
        }
        public  void stop()
        {
            if (running)
            {
                running = false;
                try { serverSocket.Close(); }
                catch (System.Exception e) { System.Console.WriteLine("serverSocket 关闭失败-" + e.Message); }
                serverSocket = null;
            }
        }
        private  void handleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);

            string requestedFile;
            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else // You can implement other methods...
            {
                notImplemented(clientSocket);
                return;
            }

            string uri = "/";
            for (int i = 0; i < requestedFile.Split("/").Length; i++)
            {
                var itm = requestedFile.Split("/")[i];
                itm = System.Web.HttpUtility.UrlDecode(itm);
                if (!System.String.IsNullOrEmpty(itm))
                {
                    uri += itm + (i < requestedFile.Split("/").Length - 1 ? "/" : "");
                }
            }
            requestedFile = uri;

            requestedFile = requestedFile.Replace("/", @"/").Replace("\\..", "");
            System.Console.WriteLine("requestedFile:" + requestedFile);

            start = requestedFile.LastIndexOf('.') + 1;
            if (start > 0)
            {
                length = requestedFile.Length - start;
                string extension = requestedFile.Substring(start, length);
                if (extensions.ContainsKey(extension)) // Do we support this extension?
                    if (File.Exists(contentPath + requestedFile)) //If yes check existence of the file
                                                                  // Everything is OK, send requested file with correct content type:
                        sendOkResponse(clientSocket,
                          File.ReadAllBytes(contentPath + requestedFile), extensions[extension]);
                    else
                        notFound(clientSocket); // We don't support this extension.
                                                // We are assuming that it doesn't exist.
            }
            else
            {
                // If file is not specified try to send index.htm or index.html
                // You can add more (default.htm, default.html)
                if (requestedFile.Substring(length - 1, 1) != @"/")
                    requestedFile += @"/";
                if (File.Exists(contentPath + requestedFile + "index.htm"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.htm"), "text/html");
                else if (File.Exists(contentPath + requestedFile + "index.html"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.html"), "text/html");
                else
                    notFound(clientSocket);
            }


        }
        private  void notImplemented(Socket clientSocket)
        {

            sendResponse(clientSocket, "<html><head><meta " +
             "http -equiv=\"Content-Type\" content=\"text/html; " +
             "charset =utf-8\">" +
             "</head><body><h2>Atasoy Simple Web " +
             "Server </h2><div>501 - Method Not " +
             "Implemented </div></body></html>",
             "501 Not Implemented", "text/html");

        }

        private  void notFound(Socket clientSocket)
        {

            sendResponse(clientSocket, "<html><head><meta " +
             "http-equiv=\"Content-Type\" content=\"text/html; " +
             "charset =utf-8\"></head><body><h2>Atasoy Simple Web " +
             "Server </h2><div>404 - Not " +
             "Found </div></body></html>",
             "404 Not Found", "text/html");
        }

        private  void sendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            sendResponse(clientSocket, bContent, "200 OK", contentType);
        }

        // For strings
        private  void sendResponse(Socket clientSocket, string strContent, string responseCode,
                                  string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            sendResponse(clientSocket, bContent, responseCode, contentType);
        }

        // For byte arrays
        private  void sendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                                  string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Atasoy Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"

                                  + "Access-Control-Allow-Methods: OPTIONS,POST,GET\r\n"
                                  + "Access-Control-Allow-Headers: x-requested-with,content-type\r\n"
                                  + "Access-Control-Allow-Origin: *\r\n"


                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch { }
        }
    }


}
