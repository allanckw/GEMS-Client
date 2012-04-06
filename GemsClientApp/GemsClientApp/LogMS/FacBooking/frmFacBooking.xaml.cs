using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using System.Windows.Controls;
using System.Data;
using System.Windows.Media;

namespace Gems.UIWPF
{

    public partial class frmFacBooking : Window
    {
        private Window mainFrame;
        private User user;
        private Event event_;
        private List<Facility> selectedFacs;

        public frmFacBooking()
        {
            InitializeComponent();
        }
        public frmFacBooking(User u, Event e, frmMain f)
            : this()
        {
            user = u;
            mainFrame = f;
            this.event_ = e;
            selectedFacs = new List<Facility>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboFaculty.ItemsSource = System.Enum.GetValues(typeof(Faculty));
            WCFHelperClient client = new WCFHelperClient();

            cboVenue.DisplayMemberPath = "FacilityID";
            cboVenue.SelectedValuePath = "FacilityID";
            client.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
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
            if (cboFaculty.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a faculty!", "Select Faculty",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            lstFacility.ItemsSource = null;
            lstFacility.Items.Clear();
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

            if (cboFaculty.SelectedIndex != -1 && cboVenue.SelectedIndex != -1)
            {
                Facility fac = (Facility)cboVenue.SelectedItem;
                if (fac.Capacity >= minCap && fac.Capacity <= maxCap)
                    lstFacility.Items.Add(fac);
            }

            else if (cboFaculty.SelectedIndex != -1 && cboVenue.SelectedIndex == -1)
                facultySearch(minCap, maxCap);

        }

        private void venueSearch(Faculty f, int minCap, int maxCap)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstFacility.ItemsSource = client.GetVenuesByCap(f,
                                        cboVenue.SelectedValue.ToString(), minCap, maxCap);
            client.Close();
        }

        private void facultySearch(int minCap, int maxCap)
        {

            WCFHelperClient client = new WCFHelperClient();
            lstFacility.ItemsSource = client.GetVenuesByFaculty((Faculty)cboFaculty.SelectedIndex, minCap, maxCap);
            client.Close();
        }
        private void cboFaculty_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cboFaculty.SelectedIndex == -1)
                return;
            cboVenue.SelectedIndex = -1;
            cboSeat.SelectedIndex = -1;

            WCFHelperClient client = new WCFHelperClient();
            cboVenue.ItemsSource = client.GetVenuesByFaculty((Faculty)cboFaculty.SelectedIndex, 0, int.MaxValue);
            client.Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lstFacility.ItemsSource = null;
            lstFacility.Items.Clear(); 
            cboFaculty.SelectedIndex = -1;
            cboVenue.SelectedIndex = -1;
            cboSeat.SelectedIndex = -1;
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (cboFaculty.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a faculty", "Select Faculty",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            selectedFacs.Clear();
            getListOfSelectedFacilities();
            if (this.selectedFacs.Count == 0)
            {
                MessageBox.Show("Please select at least 1 venue that you would like to book!",
                    "Select Venue", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var bkFacPrior = new frmFacBookingDetails(user, event_, this.selectedFacs);
            bkFacPrior.ShowDialog();

        }

        private void getListOfSelectedFacilities()
        {
            for (int i = 0; i < lstFacility.Items.Count; i++)
            {
                // Get a all list items from listbox
                ListBoxItem ListBoxItemObj = (ListBoxItem)lstFacility.ItemContainerGenerator.ContainerFromItem(lstFacility.Items[i]);

                // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                ContentPresenter ContentPresenterObj = FindVisualChild<ContentPresenter>(ListBoxItemObj);
                if (ContentPresenterObj != null)
                {
                    // call FindName on the DataTemplate of that ContentPresenter
                    DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;
                    CheckBox Chk = (CheckBox)DataTemplateObj.FindName("ChkList", ContentPresenterObj);

                    // get a selected checkbox items.
                    if (Chk.IsChecked == true)
                    {
                        selectedFacs.Add((Facility)lstFacility.Items[i]);
                    }
                }
            }
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj)
            where ChildControl : DependencyObject
        {
            if (DependencyObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
                {
                    DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                    if (Child != null && Child is ChildControl)
                    {
                        return (ChildControl)Child;
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
            }
            return null;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (lstFacility.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a facility!", "Select facility",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                frmViewFacilityDetail viewfac = new frmViewFacilityDetail(this, (Facility)lstFacility.SelectedItem);
                viewfac.ShowDialog();
            }
        }

    }
}
