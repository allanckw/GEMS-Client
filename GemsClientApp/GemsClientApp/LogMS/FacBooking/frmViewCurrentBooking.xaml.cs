using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for frmViewCurrentBooking.xaml
	/// </summary>
	public partial class frmViewCurrentBooking : Window
	{
        private Window mainFrame;
        private User user;
		public frmViewCurrentBooking()
		{
			this.InitializeComponent();
		}
        public frmViewCurrentBooking(User u, Window f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadEvents();
            cboStatus.ItemsSource = Enum.GetValues(typeof(BookingStatus));
            cboStatus.SelectedIndex = 0;
        }

        private void loadEvents()
        {
            
            List<Events> elist;
            if (user.isFacilityAdmin || user.isSystemAdmin)
            {
                EventHelper client = new EventHelper();
                elist = client.ViewAllEvents(user).ToList<Events>();
                client.Close();
            }
            else
            {
                FacilityBookingsHelper client = new FacilityBookingsHelper();
                elist = client.ViewAuthorizedEventsForFacBookings(user).ToList<Events>();
                client.Close();
            }
             
            this.cboEventList.ItemsSource = elist;

            cboEventList.SelectedValuePath = "EventID";
            cboEventList.DisplayMemberPath = "Name";
            cboEventList.SelectedIndex = 0;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
         //   loadEvents();
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

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            ClearDetail();
            retreiveBookingInformation();
        }

        private void retreiveBookingInformation()
        {
            FacilityBookingsHelper client = new FacilityBookingsHelper();

            if (cboEventList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a event!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (chkAllStatus.IsChecked.Value == false && cboStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (chkAllEventDay.IsChecked.Value == false && cboEventDay.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a day!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            EventDay evDay = (EventDay)cboEventDay.SelectedItem;
            try
            {
                this.lstRequest.ItemsSource = client.ViewFacilityBookingRequestsByEventDay(user, int.Parse(cboEventList.SelectedValue.ToString()), (BookingStatus)cboStatus.SelectedIndex,
    chkAllStatus.IsChecked.Value, chkAllEventDay.IsChecked.Value, evDay.StartDateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        private void ClearDetail()
        {
            txtEventName.Text = "";
            txtRemarks.Text = "";
            txtRequestor.Text = "";
            txtStatus.Text = "";
            lblEndTime.Content = "";
            lblStartTime.Content = "";
            FacilityBookingsHelper client = new FacilityBookingsHelper();
            //this.lstRequest.ItemsSource = client.ViewFacilityBookingRequests(user,
            //    int.Parse(cboEventList.SelectedValue.ToString()),
            //    cboStatus.SelectedIndex, chkAllStatus.IsChecked.Value,
            //    chkAllEvent.IsChecked.Value);
            client.Close();
            lvCurrentBooking.ClearSource();
            
        }

        private void lstRequest_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstRequest.SelectedIndex == -1)
            {
                //MessageBox.Show("Please Select a request!", "Invalid input",
                    //MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequest.SelectedItem;
                EventHelper client = new EventHelper();
                AdminHelper admClient = new AdminHelper();

                txtEventName.Text = client.GetEventName(fbr.EventID);
                txtRemarks.Text = fbr.Remarks;
                txtRequestor.Text = admClient.GetUserName(fbr.RequestorID);
                txtStatus.Text = fbr.Status.ToString();
                lblStartTime.Content = fbr.RequestStartDateTime.ToString("dd MMM yyyy HH:mm");
                lblEndTime.Content = fbr.RequestEndDateTime.ToString("dd MMM yyyy HH:mm");

                lvCurrentBooking.setSource(fbr);
                client.Close();

                if ((fbr.Status == BookingStatus.Pending) || (fbr.Status == BookingStatus.Approved))
                {
                    btnDrop.IsEnabled = true;
                }
                else
                {
                    btnDrop.IsEnabled = false;
                }
            }
        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (lstRequest.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a request!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequest.SelectedItem;
                FacilityBookingsHelper client = new FacilityBookingsHelper();
                if (fbr.Status == BookingStatus.Approved)
                {
                    if (MessageBox.Show("Are you sure you want to drop this confirmed booking?, It Cannot be undone!",
                        "Confirm Drop Request", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes)
                    {
                        string remarks = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Remarks for dropping", "Remarks", "");
                        client.DropConfirmedRequest(user, fbr.RequestID, fbr.EventID, remarks);
                        ClearDetail();
                    }
                }
                else if (fbr.Status == BookingStatus.Pending)
                {
                    if (MessageBox.Show("Are you sure you want to cancel this pending booking?, It Cannot be undone!",
                        "Confirm Cancel Request", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes)
                    {
                        string remarks = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Remarks for cancelling", "Remarks", "");
                        try
                        {
                            client.CancelFacilityBooking(user, fbr.RequestID, fbr.EventID, remarks);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Cancel Booking!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        finally
                        {
                            client.Close();
                        }
                        ClearDetail();
                        retreiveBookingInformation();
                    }
                }

            }
        }

        private void cboEventList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cboEventList.SelectedIndex == -1)
            {
                return;
            }

            Events ev = (Events)cboEventList.SelectedItem;
            EventHelper clientEvent = new EventHelper();
            cboEventDay.ItemsSource = clientEvent.GetDays(ev.EventID);
            cboEventDay.SelectedIndex = 0;
            clientEvent.Close();
        }

        private void chkAllStatus_Checked(object sender, RoutedEventArgs e)
        {
            cboStatus.Visibility = Visibility.Collapsed;
            //if (cboStatus==null)
            //{
            //    return;
            //}

            //if ((bool)chkAllStatus.IsChecked == true)
            //{
                
            //}
            //else
            //    cboStatus.Visibility = Visibility.Visible;

        }

        private void chkAllEventDay_Checked(object sender, RoutedEventArgs e)
        {
            cboEventDay.Visibility = Visibility.Collapsed;
            //if (cboEventDay == null)
            //{
            //    return;
            //}

            //if ((bool)chkAllEventDay.IsChecked == true)
            //{
            //    cboEventDay.Visibility = Visibility.Collapsed;
                
            //}
            //else
            //    cboEventDay.Visibility = Visibility.Visible;
        }

        private void chkAllStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            cboStatus.Visibility = Visibility.Visible;
        }

        private void chkAllEventDay_Unchecked(object sender, RoutedEventArgs e)
        {
            cboEventDay.Visibility = Visibility.Visible;
        }
	}
}