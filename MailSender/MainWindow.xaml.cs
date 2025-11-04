using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
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

namespace MailSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string server = "smpt.gmail.com";
        int port = 587;
        const string username = "test@test.com";
        const string password = "hadv dehd bsdf ocye";
        public MainWindow()
        {
            InitializeComponent();
            from.Text = username;
        }

        private void Send_btn(object sender, RoutedEventArgs e)
        {
            MailMessage msg = new MailMessage(from.Text,to.Text, subject.Text, body.Text);
            using (StreamReader sr = new StreamReader(@"mail.html"))
            {
                msg.Body = sr.ReadToEnd();
            }
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;
            msg.Attachments.Add(new Attachment(@"files/text.txt"));
            msg.Attachments.Add(new Attachment(@"files/nuts.jpg"));
            SmtpClient client = new SmtpClient(server, port);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(username, password);
            client.SendCompleted += Client_SendCompleted1;
            client.SendAsync(msg, msg);

        }

        private void Client_SendCompleted1(object sender, AsyncCompletedEventArgs e)
        {
            var state = (MailMessage)e.UserState;

            MessageBox.Show($"Message was sent! Subject: {state.Subject}!");
        }
    }
}