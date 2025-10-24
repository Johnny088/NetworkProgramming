using System.Net;
using System.Net.Sockets;
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
        string serverIp = "127.0.0.1";
        int serverPort = 4040;
        UdpClient client;
        public MainWindow()
        {
            InitializeComponent();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            client = new UdpClient();
        }
        

        private async void Send_button(object sender, RoutedEventArgs e)
        {
            
            string msg = msgText.Text;
          SendMessage(msg);

        }

        private void Leave(object sender, RoutedEventArgs e)
        {

        }

        private async void Join(object sender, RoutedEventArgs e)
        {
            string msg = "$<Join>";
            SendMessage(msg);
        }
        private async void SendMessage(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }
    }
}