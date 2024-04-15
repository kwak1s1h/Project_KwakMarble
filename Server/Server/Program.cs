using System.Net;

namespace Server
{
    class Program
    {
        private const int _port = 8080;

        static void Main(string[] args)
        {
            IPHostEntry iphost = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(iphost.AddressList[1]);
            IPAddress ipAddr = iphost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, _port); // IP주소, 포트번호 입력
        }
    }
}