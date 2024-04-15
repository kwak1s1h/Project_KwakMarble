using System.Net;
using Network;
using Server.Session;

namespace Server
{
    class Program
    {
        private const int _port = 8080;

        private static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            IPHostEntry iphost = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(iphost.AddressList[0]);
            IPAddress ipAddr = iphost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, _port);

            _listener.Init(endPoint, () => SessionManager.instance.Generate());
            Console.WriteLine($"Server is running at {endPoint}");

            while (true)
            {
                //프로그램 종료 막기 위해 while
            }
        }
    }
}