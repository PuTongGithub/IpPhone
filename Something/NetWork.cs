using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace IpPhone.Something
{
    class NetWork
    {
        private const int ServerPort = 1451;
        private Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint receive_ip = new IPEndPoint(IPAddress.Any, ServerPort);
        private Socket send_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private Controller controller;
        private string local_ip;
        Thread receive_thread;

        public NetWork(Controller c)
        {
            controller = c;
            local_ip = find_local_ip();
        }

        public void close()
        {
            receive_thread.Abort();
            send_socket.Close();
            server_socket.Close();
        }

        //局域网内用户发送广播
        public void send_broadcast(string message) {
            IPEndPoint broadcast_ip = new IPEndPoint(IPAddress.Broadcast, ServerPort);
            send_socket.SendTo(Encoding.UTF8.GetBytes(message), broadcast_ip);
        }

        //服务器配置启动
        public void server_start() {
            server_socket.Bind(receive_ip);
            send_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            receive_thread = new Thread(receive_message);
            receive_thread.Start();
        }

        //接受消息线程
        private void receive_message()
        {
            EndPoint receive_addr = (EndPoint)receive_ip;
            while (true)
            {
                byte[] data = new byte[1024];
                server_socket.ReceiveFrom(data, ref receive_addr);
                string message = Encoding.UTF8.GetString(data).TrimEnd('\u0000');
                //转发消息
                string client_ip = ((IPEndPoint)receive_addr).Address.ToString();
                if (!client_ip.Equals(local_ip))
                {
                    controller.message_process(message, client_ip);
                }
            }
        }
        
        //向指定目标发送消息
        public void send_message(string massage, string ip)
        {
            IPEndPoint send_ip = new IPEndPoint(IPAddress.Parse(ip), ServerPort);
            send_socket.SendTo(Encoding.UTF8.GetBytes(massage), send_ip);
        }

        //获取本机局域网ip地址
        private string find_local_ip()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress[] ipList = localhost.AddressList;
            foreach (var ip in ipList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

        public string get_local_ip()
        {
            return local_ip;
        }
    }
}
