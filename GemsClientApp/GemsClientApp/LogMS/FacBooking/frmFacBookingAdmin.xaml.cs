using System;
using System.Collections.Generic;
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

    public partial class frmFacBookingAdmin : Window
    {
        int ApprovedLocIdx = -1;
        private Window mainFrame;
        private User user;
        public frmFacBookingAdmin()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
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
            lstRequestor.ItemsSource = client.getAllFacilityBookingsRequest(user);
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
            //pass in venue id, grab from the watever data den pass and process
            //Just ret a list of activities for tat day for mi
            //i will code for tix part(i only nid the activites, start time and end time for tix part)
            //lvTime.SetSource(list<item(start time, end time, purpose)>) can still decide whether to leave the purpose out

            //to scroll into the index
            //lvTime.ScrollToItem();

        }

        private void lstRequestor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //fill in the details
        }
    }
}