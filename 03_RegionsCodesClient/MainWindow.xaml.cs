using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

namespace _03_RegionsCodesClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkStream ns;
        IPEndPoint ServerIP;
        TcpClient client;
        static ObservableCollection<MessagesInfo> msg; 
        StreamReader sReader;
        StreamWriter sWriter;
        public MainWindow()
        {
            InitializeComponent();
            ns = null;
            msg = new ObservableCollection<MessagesInfo>();
            sReader = null;
            sWriter = null;
            string address = ConfigurationManager.AppSettings["ServerIP"]!;
            int port = int.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            ServerIP = new IPEndPoint(IPAddress.Parse(address), port);
            client = new TcpClient();
            this.DataContext = msg;
        }

        private void connectBTN(object sender, RoutedEventArgs e)
        {
            client.Connect(ServerIP);
            ns = client.GetStream();
            sReader = new StreamReader(ns);
            sWriter = new StreamWriter(ns);

            //Receiver
        }

        private void sendBTN(object sender, RoutedEventArgs e)
        {
            string message = msgText.Text;
            sWriter.WriteLine(message);
            sWriter.Flush();
        }
    }
    public class MessagesInfo
    {

    }
}