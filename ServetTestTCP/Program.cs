using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServetTestTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8888;

            //точка подключения
            var tcpEEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); 

            //создания сокета для тсп протокола
            var tcpSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp); 

            //перевести сокет в режим ожидание, 
            //этому сокету нужно слушать конкретный  порт
            tcpSocket.Bind(tcpEEndPoint);

            //очередь подключения
            tcpSocket.Listen(5); 

            //процесс посслушивания
            while (true)
            {
                //обработчик на прием сообщений
                var listener = tcpSocket.Accept();

                //буфер куда будем принимать данные массива
                //хранилище данных (максимум)
                var buffer = new byte[256];

                //переменная реально полученных байт
                var size = 0;

                //собираем полученные данные при помощи билдера
                var data = new StringBuilder();


                //цикл с пост условием
                do
                {
                    //получаем реальное колиество байт
                    size = listener.Receive(buffer);

                    //собираем все сообщения из кусочков, после кодирования
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));

                }
                while (listener.Available > 0);

                Console.WriteLine(data); //TODO: проверить .ToString

                //даем обратный ответ (закодируем и отплавляем листенеру)
                listener.Send(Encoding.UTF8.GetBytes("Успех"));

                //выключаем подключение 
                listener.Shutdown(SocketShutdown.Both);

                //Закрываем
                listener.Close();

            }



        }
    }
}
