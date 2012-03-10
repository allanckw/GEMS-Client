using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmEventMangement.xaml
    /// </summary>
    public partial class frmEventMangement : Window
    {

        Window mainFrame;
        User user;
        public frmEventMangement()
        {
            this.InitializeComponent();
        }
        
        public frmEventMangement(User u, frmMain f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Event();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }

        private void Load_Event()
        {
            WCFHelperClient client = new WCFHelperClient();
            List<Event> list = client.ViewEvent(user).ToList<Event>();
            lstEventList.ItemsSource = list;
            client.Close();
            lstEventList.SelectedIndex = 0;
        }

        private void Load_Event(Event E)
        {
            txtEventName.Text = E.Name;
            txtDesc.Text = E.Description;
            txtWebsite.Text = E.Website;
            dtpStart.SelectedDateTime = E.StartDateTime;
            dtpEnd.SelectedDateTime = E.EndDateTime;
        }

        private void lstEventList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstEventList.SelectedIndex == -1)
                return;

            Event E = ((Event)lstEventList.SelectedItem);
            Load_Event(E);
        }

        private void btnEventEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!validateInput())
                return;

            try
            {
                DateTime startTime = dtpStart.SelectedDateTime;
                DateTime endTime = dtpEnd.SelectedDateTime;

                if (startTime.CompareTo(endTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }
                Event SelectedEvent = ((Event)lstEventList.SelectedItem);

                WCFHelperClient client = new WCFHelperClient();
                client.EditEvent(user, SelectedEvent, user.userID, txtEventName.Text, startTime, endTime, txtDesc.Text, txtWebsite.Text);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Load_Event();
            
        }

        private void btnEventCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!validateInput())
                return;
            try
            {
                DateTime startTime = dtpStart.SelectedDateTime;
                DateTime endTime = dtpEnd.SelectedDateTime;

                if (startTime.CompareTo(endTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }
                WCFHelperClient client = new WCFHelperClient();
                client.CreateEvent(user, txtEventName.Text, startTime, endTime, txtDesc.Text, txtWebsite.Text);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Load_Event();
        }

        private void btnEventDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.SelectedIndex == -1)
                return;

            Event E = ((Event)lstEventList.SelectedItem);
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                client.DeleteEvent(user, E);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Load_Event();
        }

        private bool validateInput()
        {
            if (txtEventName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter an Event Name",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpStart.SelectedDateTime == null)
            {
                MessageBox.Show("Invalid Event Start Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpEnd.SelectedDateTime == null)
            {
                MessageBox.Show("Invalid Event End Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void dtpStart_DateChanged(object sender, RoutedEventArgs e)
        {
            dtpEnd.Date = dtpStart.Date;
        }

        private void dtpEnd_DateChanged(object sender, RoutedEventArgs e)
        {
            dtpStart.Date = dtpEnd.Date;
        }

    }
}