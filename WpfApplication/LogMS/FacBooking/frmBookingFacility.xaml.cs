using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
namespace Gems.UIWPF
{

    public partial class frmBookingFacility : Window
    {
        private frmMain mainFrame;
        private User user;

        public frmBookingFacility()
        {
            InitializeComponent();
        }
        public frmBookingFacility(User u, frmMain f)
            : this()
        {
            user = u;
            mainFrame = f;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboFaculty.ItemsSource = System.Enum.GetValues(typeof(Faculty));
            WCFHelperClient client = new WCFHelperClient();
            List<Facility> list = client.getVenues(0, int.MaxValue).ToList<Facility>();
            cboVenue.ItemsSource = list;

            cboVenue.DisplayMemberPath = "FacilityID";
            cboVenue.SelectedValuePath = "FacilityID";
            client.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            lstFacility.ItemsSource = null;
            int minCap = 0, maxCap = int.MaxValue;
            switch (cboSeat.SelectedIndex)
            {
                case 0:
                    minCap = 0;
                    maxCap = 10;
                    break;
                case 1:
                    minCap = 0;
                    maxCap = 50;
                    break;
                case 2:
                    minCap = 51;
                    maxCap = 100;
                    break;
                case 3:
                    minCap = 101;
                    maxCap = 150;
                    break;
                case 4:
                    minCap = 151;
                    maxCap = 200;
                    break;
                case 5:
                    minCap = 201;
                    maxCap = 250;
                    break;
                case 6:
                    minCap = 251;
                    maxCap = 300;
                    break;
                case 7:
                    maxCap = int.MaxValue;
                    minCap = 301;
                    break;
            }

            //Both unknown
            if (cboFaculty.SelectedIndex == -1 && cboVenue.SelectedIndex == -1)
                venueSearch(minCap, maxCap);

            //Unknown Fac with known venue
            else if (cboFaculty.SelectedIndex == -1 && cboVenue.SelectedIndex != -1)
                venueSearch((Faculty)cboFaculty.SelectedIndex, minCap, maxCap);

            else if (cboFaculty.SelectedIndex != -1 && cboVenue.SelectedIndex != -1)
            {
                lstFacility.Items.Clear();
                lstFacility.Items.Add((Facility)cboVenue.SelectedItem);
            }

            else if (cboFaculty.SelectedIndex != -1 && cboVenue.SelectedIndex == -1)
                facultySearch(minCap, maxCap);
            
        }

        private void venueSearch(Faculty f, int minCap, int maxCap)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstFacility.ItemsSource = client.getVenuesByCap(f,
                                        cboVenue.SelectedValue.ToString(), minCap, maxCap);
            client.Close();
        }
        private void venueSearch(int minCap, int maxCap)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstFacility.ItemsSource = client.getVenues(minCap, maxCap);
            client.Close();
        }

        private void facultySearch(int minCap, int maxCap)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstFacility.ItemsSource = client.getVenuesByFaculty((Faculty)cboFaculty.SelectedIndex, minCap, maxCap);
            client.Close();
        }
        private void cboFaculty_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(cboFaculty.SelectedIndex == -1
            {
                return;
            }
            WCFHelperClient client = new WCFHelperClient();
            cboVenue.ItemsSource = client.getVenuesByFaculty((Faculty)cboFaculty.SelectedIndex, 0, int.MaxValue);
            client.Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cboFaculty.SelectedIndex = -1;
            cboVenue.SelectedIndex = -1;
            cboSeat.SelectedIndex = -1;
        }
    }
}