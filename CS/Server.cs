using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LouConsoleUI
{
    internal class Server
    {
        public void Send(string Question)
        {

            TcpClient client = new TcpClient();
            client.Connect("localhost", 8080);


            // Для создания соединения с сервером надо вызвать connect()
            NetworkStream tcpStream = client.GetStream();

            byte[] sendBytes = Encoding.UTF8.GetBytes(Question);
            tcpStream.Write(sendBytes, 0, sendBytes.Length);
            client.Close();

        }
    }
}
