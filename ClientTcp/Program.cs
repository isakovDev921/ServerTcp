using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8888;
           
            var tcpEEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            
            var tcpSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите сообщение");
            var message = Console.ReadLine();

            //кодируем смс
            var data = Encoding.UTF8.GetBytes(message);

            //подключаемся к сокету как клиент
            tcpSocket.Connect(tcpEEndPoint);

            //отплавляем смс
            tcpSocket.Send(data);
            
            var buffer = new byte[256];
            var size = 0;

            //ответ от сервера
            var answer = new StringBuilder();

            do
            {               
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            Console.WriteLine(answer);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();

            Console.ReadLine();
        }
    }
}
