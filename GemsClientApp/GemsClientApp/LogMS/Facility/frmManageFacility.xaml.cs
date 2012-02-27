using System;
using System.Windows;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmLocationManagement.xaml
    /// </summary>
    public partial class frmManageFacility : Window
    {
        frmMain mainFrame;
        User user;

        public frmManageFacility()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public frmManageFacility(User u, frmMain f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            cboEditFac.ItemsSource = cboFaculty.ItemsSource = System.Enum.GetValues(typeof(Faculty));
            cboFacAdmin.ItemsSource = client.getFacilityAdmins();

            //EnumRoles r = client.viewUserRole(user.userID);
            if (user.isSystemAdmin)//if (r == EnumRoles.System_Admin)
            {
                cboFacAdmin.IsEnabled = cboEditFac.IsEnabled = cboFaculty.IsEnabled = true;
                cboEditFac.SelectedIndex = cboFaculty.SelectedIndex = 0;
            }
            if (user.isSystemAdmin)//if (r == EnumRoles.System_Admin)
            {
                Faculty f = client.getFacilityAdminFaculty(user.userID);
                cboEditFac.SelectedIndex = cboFaculty.SelectedIndex = (int)f;
                cboFacAdmin.IsEnabled = cboEditFac.IsEnabled = cboFaculty.IsEnabled = false;
                cboFacAdmin.SelectedValue = user.userID;

            }
            
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

        private void lstVenue_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Facility f = (Facility)lstVenue.SelectedItem;
            if (f != null)
            {
                cboFaculty.SelectedIndex = (int)f.Faculty;
                txtVenue.Text = f.FacilityID;
                txtTechContact.Text = f.TechContact;
                txtSeatCapacity1.Text = f.Capacity.ToString();
                cboFacAdmin.SelectedValue = f.BookingContact;
                txtLocation.Text = f.Location;
            }

        }

        private void cboFaculty_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            lstVenue.ItemsSource = client.getVenuesByFaculty((Faculty)cboFaculty.SelectedIndex, 0, int.MaxValue);
            client.Close();

            cboEditFac.SelectedIndex = cboFaculty.SelectedIndex;
            txtVenue.Text = "";
            txtTechContact.Text = "";
            txtSeatCapacity1.Text = "";
            if (user.isSystemAdmin)
            {
                cboFacAdmin.SelectedValue = -1;
            }
            txtLocation.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int cap;
            bool parseSuccess = Int32.TryParse(txtSeatCapacity1.Text.Trim(), out cap);

            if (cboFacAdmin.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the facility admin!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            WCFHelperClient client = new WCFHelperClient();

            if (parseSuccess)
            {
                try
                {
                    client.updateFacility(user, txtVenue.Text.Trim(), (Faculty)cboEditFac.SelectedIndex,
                    txtLocation.Text.Trim(), cboFacAdmin.SelectedValue.ToString(),
                    txtTechContact.Text.Trim(), cap);
                    MessageBox.Show("Facility successfully updated", "Update Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    lstVenue.ItemsSource = client.getVenuesByFaculty((Faculty)cboFaculty.SelectedIndex,0, int.MaxValue);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error have occured: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                client.Close();
            }
            else
            {
                MessageBox.Show("Seat Capacity must be a numeric value!", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                client.removeFacility(user, txtVenue.Text.Trim(), (Faculty)cboFaculty.SelectedIndex);
                MessageBox.Show("Facility successfully deleted", "Delete Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                lstVenue.ItemsSource = client.getVenuesByFaculty((Faculty)cboFaculty.SelectedIndex,0, int.MaxValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error have occured: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }


    }
}