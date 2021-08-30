using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Optical_View.Class
{
    class SystemOperation
    {
        /// <summary>        
        /// 获取操作系统已用的端口号        
        /// </summary>        
        /// <returns></returns>        
        public static int PortIsUsed()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
