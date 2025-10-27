using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_messenger_client
{
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        UdpClient client;
        static ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        public MainWindow()
        {
            InitializeComponent();
            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            int port = int.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            client = new UdpClient();
            this.DataContext = messages;
        }


        private async void Send_button(object sender, RoutedEventArgs e)
        {

            string msg = msgText.Text;
            SendMessage(msg);

        }

        private void Leave(object sender, RoutedEventArgs e)
        {
            string msg = "$leave";
            SendMessage(msg);
        }

        private async void Join(object sender, RoutedEventArgs e)
        {
            string msg = "$<join>";
            SendMessage(msg);
            Receiver();
        }

        private async void SendMessage(string msg)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }

        private async Task Receiver()
        {
            while (true)
            {
                var res = await client.ReceiveAsync();
                string message = Encoding.Unicode.GetString(res.Buffer);
                messages.Add(new MessageInfo(message, DateTime.Now));
            }
        }
    }
    public class MessageInfo
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string M, DateTime T)
        {
            Message = M;
            Time = T;
        }
        public override string ToString()
        {
            return $"{Message}. Time:{Time.ToLocalTime()}";
        }
    }
}