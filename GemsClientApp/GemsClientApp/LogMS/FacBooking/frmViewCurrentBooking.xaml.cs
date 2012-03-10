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
        }

        private void loadEvents()
        {
            WCFHelperClient client = new WCFHelperClient();
            List<Event> elist;
            if (user.isFacilityAdmin || user.isSystemAdmin)
            {
                elist =client.viewAllEvents(user).ToList<Event>();
            }
            else
            {
               elist = client.ViewEvent(user).ToList<Event>();
            }
             
            this.cboEventList.ItemsSource = elist;
            client.Close();
            cboEventList.SelectedIndex = 0;
            cboEventList.SelectedValuePath = "EventID";
            cboEventList.DisplayMemberPath = "Name";
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadEvents();
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
            WCFHelperClient client = new WCFHelperClient();

            if (chkAllStatus.IsChecked.Value == false && cboStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (chkAllEvent.IsChecked.Value == false && cboEventList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a event!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            this.lstRequest.ItemsSource = client.viewFacilityBookingRequests(user, 
                int.Parse(cboEventList.SelectedValue.ToString()),
                cboStatus.SelectedIndex, chkAllStatus.IsChecked.Value,
                chkAllEvent.IsChecked.Value);
            
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
            WCFHelperClient client = new WCFHelperClient();
            this.lstRequest.ItemsSource = client.viewFacilityBookingRequests(user,
                int.Parse(cboEventList.SelectedValue.ToString()),
                cboStatus.SelectedIndex, chkAllStatus.IsChecked.Value,
                chkAllEvent.IsChecked.Value);
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
                WCFHelperClient client = new WCFHelperClient();

                txtEventName.Text = client.getEventName(fbr.EventID);
                txtRemarks.Text = fbr.Remarks;
                txtRequestor.Text = client.getUserName(fbr.RequestorID);
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
            }
            else
            {
                FacilityBookingRequest fbr = (FacilityBookingRequest)lstRequest.SelectedItem;
                WCFHelperClient client = new WCFHelperClient();
                if (fbr.Status == BookingStatus.Approved)
                {
                    if (MessageBox.Show("Are you sure you want to drop this confirmed booking?, It Cannot be undone!",
                        "Confirm Drop Request", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes)
                    {
                        string remarks = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Remarks for dropping", "Remarks", "");
                        client.dropConfirmedRequest(user, fbr.RequestID, fbr.EventID, remarks);
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
                        client.cancelFacilityBooking(user, fbr.RequestID, fbr.EventID, remarks);
                        ClearDetail();
                    }
                }

            }
        }
	}
}