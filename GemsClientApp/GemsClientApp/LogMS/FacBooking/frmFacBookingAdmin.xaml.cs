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
            loadRequests();
        }

        private void loadRequests()
        {
            WCFHelperClient client = new WCFHelperClient();
            lstRequestor.ItemsSource = client.GetFacBookingRequestList(user);
            client.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
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
            if (lstRequestor.SelectedIndex != -1)
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
                fbcList = client.GetActivitiesForDay(user, day,
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
                //MessageBox.Show("Please select a booking request", "No request selected",
                    //MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            try
            {
                
                FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;
                WCFHelperClient client = new WCFHelperClient();
                AdminHelper admClient = new AdminHelper();
                txtEventName.Text = client.GetEventName(fbr.EventID);
                txtRequestor.Text = admClient.GetUserName(fbr.RequestorID);
                lblStartTime.Content = fbr.RequestStartDateTime.ToString("dd MMM yyyy HH:mm");
                lblEndTime.Content = fbr.RequestEndDateTime.ToString("dd MMM yyyy HH:mm");

                dgLocation.ItemsSource = fbr.RequestDetails;
                client.Close();
                lvTimeslot.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgLocation.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking request", "No request selected",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!lvTimeslot.CanApproved())
            {
                MessageBox.Show("There is a clash for this timeslot", "Timeslot Clash",
                   MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            WCFHelperClient client = new WCFHelperClient();
            FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequestor.SelectedItem;

            try
            {
                FacilityBookingRequestDetails fbrd = (FacilityBookingRequestDetails)dgLocation.SelectedItem;
                client.ApproveFacilityBooking(user, fbr.RequestID, fbr.EventID, fbrd.RequestDetailsID,
                    txtRemarks.Text.Trim(), txtEventName.Text.Trim());
                MessageBox.Show("Request have been approved!", "Approved",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                loadRequests();
                dgLocation.ItemsSource = null;
                lvTimeslot.Reset();
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
                client.RejectFacilityBooking(user, fbr.RequestID, fbr.EventID, txtRemarks.Text.Trim());
                MessageBox.Show("Request have been rejected!", "Rejected",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                loadRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error have occured with the following message: " + ex.Message,
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

        private void dgLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLocation.SelectedIndex == -1)
            {
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
                fbcList = client.GetActivitiesForDay(user, day,
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

    }
}