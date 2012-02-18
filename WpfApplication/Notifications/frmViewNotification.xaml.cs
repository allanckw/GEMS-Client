using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmViewNotification.xaml
    /// </summary>
    public partial class frmViewNotification : Window
    {

        Notifications note;
        User user;

        public frmViewNotification()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EvmsServiceClient client = new EvmsServiceClient();
            txtSenderName.Text = client.searchUser("", note.Sender).FirstOrDefault<User>().Name;
            txtSenderID.Text = note.Sender;
            txtMessage.AppendText(note.Message);
            txtSubject.Text = note.Title;

            client.Close();
        }

        public frmViewNotification(Notifications n, User u)
            : this()
        {

            this.note = n;
            this.user = u;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string message = txtReply.Text.Trim();
            string title = txtSubject.Text.Trim();
            if (!title.StartsWith("RE:"))
            {
                title = "RE: " + title;
            }
            EvmsServiceClient client = new EvmsServiceClient();
            try
            {
                client.sendNotification(user.userID, txtSenderID.Text, title, message);
                client.Close();
                MessageBox.Show("Replied Sent", "Sent", MessageBoxButton.OK, MessageBoxImage.Information);
                btnExit_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured with the error message: " + ex.Message,
                "Error Sending Reply", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
