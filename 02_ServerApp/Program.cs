using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _02_ServerApp
{
    class Server
    {
        public List<IPEndPoint> Members { get; set; }
           public int port = 4040;
           public const string JOIN_CMD = "$<join>";
           public UdpClient udpClient;
        public Server()
        {
            Members = new List<IPEndPoint>();
            udpClient = new UdpClient(port);
        }
        public void AddMembers(IPEndPoint member)
        {
            Members.Add(member);
            Console.WriteLine();

        }
        public void SendAll()
        {

        }

    }

    internal class Program
    {
       
        static void Main(string[] args)
        {
            string ip;
            IPEndPoint clientEndPoint = null;
            while(true)
            {
                byte[] data = udpClient.Receive(ref clientEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Message: {message} From {clientEndPoint}");
                Server server = new Server();
                switch(message)
                {
                    case JOIN_CMD:
                        server.members.Add(clientEndPoint);
                        Console.WriteLine("Members was added");
                        break;
                    default:
                        SendAll();
                        break;
                }
            }
           
            
        }
    }

}
