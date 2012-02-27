using System;
using System.Windows;
using System.Windows.Input;
using evmsService.entities;
namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmLocationAdminAdd.xaml
    /// </summary>
    public partial class frmAddFacility : Window
    {
        frmMain mainFrame;
        User user;

        public frmAddFacility()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public frmAddFacility(User u, frmMain f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            //EnumRoles r = client.viewUserRole(user.userID);
            cboFaculty.ItemsSource = System.Enum.GetValues(typeof(Faculty));
            cboFacAdmin.ItemsSource = client.getFacilityAdmins();

            if (user.isSystemAdmin)//if (r == EnumRoles.System_Admin)
            {
                cboFacAdmin.IsEnabled = cboFaculty.IsEnabled = true;
                 
            }
            if (user.isSystemAdmin)//if (r == EnumRoles.System_Admin)
            {
                Faculty f = client.getFacilityAdminFaculty(user.userID);
                cboFaculty.SelectedIndex = (int)f;
                cboFacAdmin.SelectedValue = user.userID;
                cboFaculty.IsEnabled = false;
                cboFacAdmin.IsEnabled = false;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int cap;
            bool parseSuccess = Int32.TryParse(txtSeatCapacity1.Text.Trim(), out cap);

            if (parseSuccess)
            {
                try
                {
                    WCFHelperClient client = new WCFHelperClient();
                    client.addFacility(user, txtVenue.Text.Trim(), (Faculty)cboFaculty.SelectedIndex,
                        txtLocation.Text.Trim(),cboFacAdmin.SelectedValue.ToString(), txtTechContact.Text.Trim(), cap);

                    client.Close();
                    MessageBox.Show("Facility have been added successfully", "Facility added", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error have occured: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seat Capacity must be a numeric value!", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }



    }
}