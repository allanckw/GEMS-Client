using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{

    public partial class frmFacBookingAdmin : Window
    {
        private int ApprovedLocIdx = -1;
        private Window mainFrame;
        private User user;
        public frmFacBookingAdmin()
        {
            this.InitializeComponent();
        }

        public frmFacBookingAdmin(User u, Window f)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: Fill in the requestor details

            WCFHelperClient client = new WCFHelperClient();
            lstRequestor.ItemsSource = client.getFacBookingRequestList(user);
            client.Close();
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

        private void radApproved_Click(object sender, RoutedEventArgs e)
        {
            ApprovedLocIdx = dgLocation.SelectedIndex;
        }

        private void btnViewTimeslot_Click(object sender, RoutedEventArgs e)
        {
            if (lstRequestor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking request", "No request selected",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;
            FacilityBookingRequestDetails fbrDetails = (FacilityBookingRequestDetails)dgLocation.SelectedItem;
            DateTime day = fbr.RequestStartDateTime;
            day = day.AddHours(-day.Hour).AddMinutes(-day.Minute);

            List<FacilityBookingConfirmed> fbcList;
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                fbcList = client.getActivitiesForDay(user, day,
                     fbrDetails.Faculty, fbrDetails.FacilityID).ToList<FacilityBookingConfirmed>();
                lvTimeslot.SetBookingTimeRange(fbr.RequestStartDateTime, fbr.RequestEndDateTime);
                lvTimeslot.SetSource(fbcList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void lstRequestor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRequestor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking request", "No request selected",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            try
            {
                FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;
                WCFHelperClient client = new WCFHelperClient();
                txtEventName.Text = client.getEventName(fbr.EventID);
                txtRequestor.Text = client.getUserName(fbr.RequestorID);
                lblStartTime.Content = fbr.RequestStartDateTime.ToString("dd MMM yyyy HH:mm");
                lblEndTime.Content = fbr.RequestEndDateTime.ToString("dd MMM yyyy HH:mm");

                dgLocation.ItemsSource = fbr.RequestDetails;
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (lstRequestor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking request", "No request selected",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            WCFHelperClient client = new WCFHelperClient();
            FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;

            try
            {
                FacilityBookingRequestDetails fbrd = (FacilityBookingRequestDetails)dgLocation.SelectedItem;
                client.approveFacilityBooking(user, fbr.RequestID, fbr.EventID, fbrd.RequestDetailsID,
                    txtRemarks.Text.Trim(), txtEventName.Text.Trim());
                MessageBox.Show("Request have been approved!", "Approved",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error have occured with the following message: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (lstRequestor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking request", "No request selected",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            WCFHelperClient client = new WCFHelperClient();
            FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;

            try
            {
                client.rejectFacilityBooking(user, fbr.RequestID, fbr.EventID, txtRemarks.Text.Trim());
                MessageBox.Show("Request have been rejected!", "Rejected",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error have occured with the following message: " + ex.Message,
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

    }
}