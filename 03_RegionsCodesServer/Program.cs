using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace _03_RegionsCodesServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
           Server server = new Server();
            server.Run();
        }
    }
    class Server
    {
        TcpClient client;
        string ip;
        int port;
        TcpListener receiver;
        string msg;
        Dictionary<string, string> code;
        public Server()
        {
            client = null;
            ip = "127.0.0.1";
            port = 4040;
            receiver = new TcpListener(new IPEndPoint(IPAddress.Parse(ip),port));
            msg = "";
            code = new Dictionary<string, string>()
            {{"AC","Volyn" },
                {"KC", "Volyn" },
                {"BC", "Lviv"},
                {"HC", "Lviv"},
                {"AO","Zakarpattia" },
                {"KO","Zakarpattia" },
                {"BK","Rivne" },
                {"HK","Rivne" },
                {"BO","Ternopil" },
                {"HO","Ternopil" },
                {"AT","Ivano-Frankivsk" },
                {"KT","Ivano-Frankivsk" },
                {"CE","Chernivtsi" },
                {"IE","Chernivtsi" },
                {"BX","RivneKhelnytskyi" },
                {"HX","RivneKhelnytskyi" },
                {"AM","Zhytomyr" },
                {"KM","Zhytomyr" },
                {"AB","Vinnytsia" },
                {"KB","Vinnytsia" },
                {"AA","Kyiv" },
                {"KA","Kyiv" },
                {"CA","Cherkasy" },
                {"IA","Cherkasy" },
                {"BA","Kropyvnytskyi" },
                {"HA","Kropyvnytskyi" },
                {"BE","MYkolaiv" },
                {"HE","MYkolaiv" },
                {"BH","Odesa" },
                {"HH","Odesa" },
                {"CB","Chernihiv" },
                {"IB","Chernihiv" },
                {"BM","Sumy" },
                {"NM","Sumy" },
                {"BI","Poltava" },
                {"HI","Poltava" },
                {"AE","Dnipro" },
                {"KE","Dnipro" },
                {"AP","Zaporizhia" },
                {"KP","Zaporizhia" },
                {"BT","Kherson" },
                {"HT","Kherson" },
                {"AK","Crimea" },
                {"KK","Crimea" },
                {"AX","Kharkiv" },
                {"KX","Kharkiv" },
                {"BB","Luhansk" },
                {"HB","Luhansk" },
                {"AH","Donetsk" },
                {"KH","Donetsk" },

            };
        }
        public void Run()
        {
            receiver.Start();
            Console.WriteLine("server is running...");
            client = receiver.AcceptTcpClient();
            Console.WriteLine("connection is successful");
            NetworkStream ns = client.GetStream();
            StreamReader streamReader = new StreamReader(ns);
            StreamWriter streamWriter = new StreamWriter(ns);
            while (true)
            {
                msg = streamReader.ReadLine();
                if (msg == null)
                {
                    Console.WriteLine("the client disconnected");
                    break;
                }
                else
                {
                    Console.WriteLine($"Query: {msg} from {client.Client.LocalEndPoint}");
                    checkCode();
                    streamWriter.WriteLine(msg);
                    streamWriter.Flush();
                }
                    
            }
            
        }
        private void checkCode()
        {
            if (code.ContainsKey(msg.ToUpper()))
            {
                msg = code[msg.ToUpper()];
            }
            else
                msg = "The code isn't found";
        }

    }
}
