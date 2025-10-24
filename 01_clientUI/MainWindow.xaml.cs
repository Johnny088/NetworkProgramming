using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _01_clientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string address = "127.0.0.1";
        static int port = 8080;
        UdpClient client;
        IPEndPoint remoteServer;
        IPEndPoint receiver;
        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient();
            remoteServer = new IPEndPoint(IPAddress.Parse(address),port);
            receiver = new IPEndPoint(IPAddress.Any, 0);
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            //sending msg to server
            string msg = input.Text;
            byte[] data = Encoding.Unicode.GetBytes(msg);
            client.Send(data, data.Length, remoteServer);
            //waiting answer from the server
            data = client.Receive(ref receiver);
            inputList.Items.Add(msg);
            msg = Encoding.Unicode.GetString(data);
            output.Items.Add(msg);
        }
    }
    
}