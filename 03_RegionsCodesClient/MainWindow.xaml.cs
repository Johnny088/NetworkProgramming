using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Windows;


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
        static ObservableCollection<string> msg; 
        StreamReader sReader;
        StreamWriter sWriter;
        public MainWindow()
        {
            InitializeComponent();
            ns = null;
            msg = new ObservableCollection<string>();
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
            try
            {
                client.Connect(ServerIP);
                ns = client.GetStream();
                sReader = new StreamReader(ns);
                sWriter = new StreamWriter(ns);

                Receiver();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task Receiver()
        {
            while (true)
            {
                msg.Add(await sReader.ReadLineAsync());
            }
        }

        private void sendBTN(object sender, RoutedEventArgs e)
        {
            string message = msgText.Text;
            sWriter.WriteLine(message);
            sWriter.Flush();
        }

        private void disconnectBTN(object sender, RoutedEventArgs e)
        {
            ns.Close();
            client.Close();
        }
    }
}