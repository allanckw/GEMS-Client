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
    /// Interaction logic for frmNotificationReader.xaml
    /// </summary>
    public partial class frmNotificationList : Window
    {
        User user;
        Window mainFrame;

        public frmNotificationList()
        {
            InitializeComponent();
        }

        public frmNotificationList(User u, Window f)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstMsg.ItemsSource = client.getUnreadMessages(user.userID);
            client.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //mainFrame.Visibility = Visibility.Visible;
        }

        private void radUnread_Checked(object sender, RoutedEventArgs e)
        {
            if (user == null)
                return;

            WCFHelperClient client = new WCFHelperClient();
            if (radUnread.IsChecked == true)
            { 
                lstMsg.ItemsSource = client.getUnreadMessages(user.userID);
            }
            else
            {
                lstMsg.ItemsSource = client.getAllMessages(user.userID);
            }
            client.Close();
        }

        private void btnVIew_Click(object sender, RoutedEventArgs e)
        {
            Notifications n = (Notifications)lstMsg.SelectedItem;
            if (!n.isRead)
            {
                WCFHelperClient client = new WCFHelperClient();
                client.setNotificationRead(n);
                client.Close();
            }
            var viewNote = new frmViewNotification(n, user);
            viewNote.ShowDialog();
        }


        private void btnDeleteSel_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this message?", "Confirm deletion",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Notifications n = (Notifications)lstMsg.SelectedItem;
                    client.deleteNotifications(n);
                    radUnread_Checked(this, e);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured with the message: " +  ex.Message, 
                    "Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Close();
            }
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                if (MessageBox.Show("Are you sure you want to delete all messages in your inbox?", "Confirm deletion",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    client.deleteAllNotificationsOfUser(user.userID);
                    radUnread_Checked(this, e);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured with the message: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
