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
            CreateDTPData();
            Load_Event();
            dtpStart.DisplayDateStart = DateTime.Now;
            dtpEnd.DisplayDateStart = DateTime.Now;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void CreateDTPData()
        {
            for (int i = 0; i <= 23; i++)
            {
                cboStartHr.Items.Add(string.Format("{0:00}", i));
                cboEndHr.Items.Add(string.Format("{0:00}", i));
            }

            for (int i = 0; i <= 55; i += 30)
            {
                cboStartMin.Items.Add(string.Format("{0:00}", i));
                cboEndMin.Items.Add(string.Format("{0:00}", i));
            }
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
            dtpStart.SelectedDate = E.StartDateTime;
            dtpEnd.SelectedDate = E.EndDateTime;
            cboStartHr.SelectedIndex = E.StartDateTime.Hour;
            cboStartMin.SelectedIndex = E.StartDateTime.Minute / 30;
            cboEndHr.SelectedIndex = E.EndDateTime.Hour;
            cboEndMin.SelectedIndex = E.EndDateTime.Minute / 30;
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
                DateTime startTime = dtpStart.SelectedDate.Value;
                DateTime endTime = dtpEnd.SelectedDate.Value;
                startTime = startTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
                startTime = startTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
                endTime = endTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
                endTime = endTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));

                if (startTime.CompareTo(endTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }
                Event SelectedEvent = ((Event)lstEventList.SelectedItem);

                WCFHelperClient client = new WCFHelperClient();
                client.EditEvent(user, SelectedEvent, user.userID, txtEventName.Text, startTime, endTime, txtDesc.Text, txtWebsite.Text);
                client.Close();
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
                DateTime startTime = dtpStart.SelectedDate.Value;
                DateTime endTime = dtpEnd.SelectedDate.Value;
                startTime = startTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
                startTime = startTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
                endTime = endTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
                endTime = endTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));

                if (startTime.CompareTo(endTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }
                WCFHelperClient client = new WCFHelperClient();
                client.CreateEvent(user, txtEventName.Text, startTime, endTime, txtEventName.Text, txtWebsite.Text);
                client.Close();
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
            else if (dtpStart.SelectedDate == null)
            {
                MessageBox.Show("Invalid Event Start Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpEnd.SelectedDate == null)
            {
                MessageBox.Show("Invalid Event End Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (cboStartHr.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Event Start Hour",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (cboStartMin.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Event Start Minute",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (cboEndHr.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Event End Hour",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (cboEndMin.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Event End Minute",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }
    }
}