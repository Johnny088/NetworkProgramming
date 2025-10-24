using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace _01_Server
{
    internal class Program
    {

        static void Main(string[] args)
        {

            MessengerData messenger = new MessengerData();
            messenger.runingServer();
        }



        class MessengerData
        {
            public Dictionary<string, string> MsgData { get; set; }
            public string FileName { get; set; }
            public string JsonString { get; set; }
            public IPEndPoint Server { get; set; }

            public IPEndPoint RemoteIpPoint;
            public string TempKey { get; set; }
            public bool Flag { get; set; }

            public UdpClient UdpReceiver { get; set; }
            const string address = "127.0.0.1";
            const int port = 8080;
            public MessengerData()
            {
                FileName = "data.json";
                MsgData = new Dictionary<string, string>();
                Server = new IPEndPoint(IPAddress.Parse(address), port);
                RemoteIpPoint = new IPEndPoint(IPAddress.Any, 0);
                UdpReceiver = new UdpClient(Server);
                TempKey = null;
                Flag = false;
            }
            public void read()
            {
                if (!File.Exists(FileName))
                {
                    File.WriteAllText(FileName, "{}");
                    Console.WriteLine("the data is empty, you can add some patterns");
                }
                else
                {
                    JsonString = File.ReadAllText(FileName);
                    MsgData = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonString)!;
                    Console.WriteLine("The data is read");
                }
            }
            public void pushToBase()
            {
                JsonString = JsonSerializer.Serialize(MsgData);
                File.WriteAllText(FileName, JsonString);
            }
            public void chat(string msg)
            {

                if (MsgData.ContainsKey(msg))
                {
                    byte[] temp;
                    temp = Encoding.Unicode.GetBytes(MsgData[msg]);
                    UdpReceiver.Send(temp, temp.Length, RemoteIpPoint);
                }
                else if (!MsgData.ContainsKey(msg) && Flag == false)
                {
                    TempKey = msg;
                    byte[] temp;
                    string request = "\nHey mate, how should I response?";
                    temp = Encoding.Unicode.GetBytes(request);
                    UdpReceiver.Send(temp, temp.Length, RemoteIpPoint);
                    Flag = true;
                    return;
                }
                if (Flag == true)
                {
                    byte[] temp;
                    string response = "\nItem was added";
                    temp = Encoding.Unicode.GetBytes(response);
                    UdpReceiver.Send(temp, temp.Length, RemoteIpPoint);
                    MsgData.TryAdd(TempKey, msg);
                    //Dictionary<string, string> dictItem = new Dictionary<string, string>();
                    //dictItem.Add(TempKey, msg);
                    //writeToFile(dictItem);
                    writeToFile();
                    Flag = false;
                }

            }
            //public void writeToFile(Dictionary<string, string> dictItem)
            //{
            //    JsonString = JsonSerializer.Serialize(dictItem);
            //    File.WriteAllText(FileName, JsonString);

            //}
            public void writeToFile()
            {
                JsonString = JsonSerializer.Serialize(MsgData);
                File.WriteAllText(FileName, JsonString);

            }
            public void runingServer()
            {
                try
                {
                    read();
                    Console.WriteLine("The server is running...");
                    while (true)
                    {
                        byte[] data = UdpReceiver.Receive(ref RemoteIpPoint);
                        string msg = Encoding.Unicode.GetString(data);
                        chat(msg);

                        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {msg} from {RemoteIpPoint} ");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                finally
                {
                    UdpReceiver.Close();
                }

            }
        }
    }
}
