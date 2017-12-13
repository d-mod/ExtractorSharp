using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ExtractorSharp.Host.Http;
using Hprose.Server;

namespace ExtractorSharp.Host {
    class Program{
       
        static void Main(string[] args) {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 80));
            socket.Listen(200);
            socket.BeginAccept(OnAccept, socket);
            Console.Read();
        }

        private static void OnAccept(IAsyncResult ar) {
            var next = ar.AsyncState as Socket;
            var last = next.EndAccept(ar);  //接收到来自浏览器的代理socket
                                            //NO.1  并行处理http请求
            next.BeginAccept(OnAccept, next); //开始下一次http请求接收   （此行代码放在NO.2处时，就是串行处理http请求，前一次处理过程会阻塞下一次请求处理）

            var  buf = new byte[last.Available];
            var length = last.Receive(buf);  //接收浏览器的请求数据
            var content = Encoding.UTF8.GetString(buf, 0, length).Trim();
            var request = HttpRequest.Create(content);
        }
    }
}
