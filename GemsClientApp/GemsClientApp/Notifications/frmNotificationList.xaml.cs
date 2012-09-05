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
            NotifHelper client = new NotifHelper();
            lstMsg.ItemsSource = client.GetUnreadMessages(user, user.UserID);
            client.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //mainFrame.Visibility = Visibility.Visible;
        }

        private void radUnread_Checked(object sender, RoutedEventArgs e)
        {
            if (user == null)
                return;

            NotifHelper client = new NotifHelper();
            if (radUnread.IsChecked == true)
            {
                lstMsg.ItemsSource = client.GetUnreadMessages(user, user.UserID);
            }
            else
            {
                lstMsg.ItemsSource = client.GetAllMessages(user, user.UserID);
            }
            client.Close();
        }

        private void btnVIew_Click(object sender, RoutedEventArgs e)
        {
            if (lstMsg.SelectedIndex != -1)
            {
                Notifications n = (Notifications)lstMsg.SelectedItem;
                if (!n.isRead)
                {
                    NotifHelper client = new NotifHelper();
                    client.SetNotificationRead(user, n);
                    client.Close();
                }
                var viewNote = new frmViewNotification(n, user);
                viewNote.ShowDialog();
            }
            else
            {
                MessageBox.Show("You need to select a message");
            }
        }

        private void btnDeleteSel_Click(object sender, RoutedEventArgs e)
        {
            if (lstMsg.SelectedIndex != -1)
            {
                NotifHelper client = new NotifHelper();
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this message?", "Confirm deletion",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Notifications n = (Notifications)lstMsg.SelectedItem;
                        client.DeleteNotifications(user, n);
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
            else
            {
                MessageBox.Show("You need to select a message");
            }
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            NotifHelper client = new NotifHelper();
            try
            {
                if (MessageBox.Show("Are you sure you want to delete all messages in your inbox?", "Confirm deletion",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    client.DeleteAllNotificationsOfUser(user, user.UserID);
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
