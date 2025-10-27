using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _02_ServerApp
{
    class Server
    {
        List<IPEndPoint> members { get; set; }
        IPEndPoint clientEndPoint = null;
        int port = 4040;
        const string JOIN_CMD = "$<join>";
        const string Leave_CMD = "$leave";
        UdpClient server_udp;
        public Server()
        {
            members = new List<IPEndPoint>();
            server_udp = new UdpClient(port);
        }
        public void AddMembers(IPEndPoint member)
        {
            members.Add(member);
            Console.WriteLine($"Member {member} was added ");

        }
        public void start()
        {
            while (true)
            {
                byte[] data = server_udp.Receive(ref clientEndPoint);
                string message = Encoding.Unicode.GetString(data);
                switch (message)
                {
                    case JOIN_CMD:
                        AddMembers(clientEndPoint);
                        break;
                    case Leave_CMD:
                        Leave(clientEndPoint);
                        break;
                    default:
                        Console.WriteLine($"Message : {message}. From : {clientEndPoint}");
                        SendAll(data);
                        break;
                }
            }

        }
        public void Leave(IPEndPoint client)
        {
            members.Remove(client);
            Console.WriteLine($"the {client} left the group.");
        }
        public void SendAll(byte[] data)
        {
            foreach (IPEndPoint member in members)
            {
                server_udp.SendAsync(data, data.Length, member);
            }
        }

    }

    internal class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server();
            server.start();


        }
    }

}
