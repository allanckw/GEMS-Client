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
            CreateDTPData();
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

        public frmEventMangement(User u, frmMain f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
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

        private void btnEventEdit_Click(object sender, RoutedEventArgs e)
        {
            //int selectedindex = lstEventList.SelectedIndex;
            try
            {
                WCFHelperClient client = new WCFHelperClient();

                
                if (txtEventName.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter an Event Name");
                    return;
                }

                if (dtpStart.SelectedDate == null)
                {
                    MessageBox.Show("Invalid Event Start Date");
                    return;
                }
                if (dtpEnd.SelectedDate == null)
                {
                    MessageBox.Show("Invalid Event End Date");
                    return;
                }
                if (cboStartHr.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event Start Hour");
                    return;
                }
                if (cboStartMin.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event Start Minute");
                    return;
                }
                if (cboEndHr.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event End Hour");
                    return;
                }
                if (cboEndMin.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event End Minute");
                    return;
                }
                DateTime EventstartDateTime = dtpStart.SelectedDate.Value;
                DateTime EventEndDateTime = dtpEnd.SelectedDate.Value;
                EventstartDateTime = EventstartDateTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
                EventstartDateTime = EventstartDateTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
                EventEndDateTime = EventEndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
                EventEndDateTime = EventEndDateTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));


                if (EventstartDateTime.CompareTo(EventEndDateTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }
                Event SelectedEvent = ((Event)lstEventList.SelectedItem);
                client.EditEvent(user, SelectedEvent, user.userID, txtEventName.Text, EventstartDateTime, EventEndDateTime, txtDesc.Text, txtWebsite.Text);
               // client.CreateEvent(user, txtEventName.Text, EventstartDateTime, EventEndDateTime, txtEventName.Text, txtWebsite.Text);

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Load_Event();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Event();
        }

        private void btnEventCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();

                if (txtEventName.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter an Event Name");
                    return;
                }

                if (dtpStart.SelectedDate == null)
                {
                    MessageBox.Show("Invalid Event Start Date");
                    return;
                }
                if (dtpEnd.SelectedDate == null)
                {
                    MessageBox.Show("Invalid Event End Date");
                    return;
                }
                if (cboStartHr.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event Start Hour");
                    return;
                }
                if (cboStartMin.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event Start Minute");
                    return;
                }
                if (cboEndHr.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event End Hour");
                    return;
                }
                if (cboEndMin.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Event End Minute");
                    return;
                }
                DateTime EventstartDateTime = dtpStart.SelectedDate.Value;
                DateTime EventEndDateTime = dtpEnd.SelectedDate.Value;
                EventstartDateTime = EventstartDateTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
                EventstartDateTime = EventstartDateTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
                EventEndDateTime = EventEndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
                EventEndDateTime = EventEndDateTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));


                if (EventstartDateTime.CompareTo(EventEndDateTime) >= 0)
                {
                    MessageBox.Show("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    return;
                }

                client.CreateEvent(user, txtEventName.Text, EventstartDateTime, EventEndDateTime, txtEventName.Text, txtWebsite.Text);

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Load_Event();
        }
        private void Load_Event(Event E)
        {
            txtEventName.Text = E.Name;
            txtDesc.Text = E.Description;
            txtWebsite.Text = E.Website;
            dtpStart.SelectedDate = E.StartDateTime;
            dtpEnd.SelectedDate = E.EndDateTime;
            cboStartHr.SelectedIndex = E.StartDateTime.Hour;
            cboStartMin.SelectedIndex = E.StartDateTime.Minute/30;
            cboEndHr.SelectedIndex = E.EndDateTime.Hour;
            cboEndMin.SelectedIndex = E.EndDateTime.Minute/30;

        }

        private void lstEventList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
            if (lstEventList.SelectedIndex == -1)
                return;

            Event E = ((Event)lstEventList.SelectedItem);

            Load_Event(E);

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
    }
}