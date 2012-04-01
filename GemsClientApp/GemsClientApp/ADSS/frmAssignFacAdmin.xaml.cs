using System;
using System.Windows;
using System.Windows.Input;
using evmsService.Controllers;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmAssignFacilityAdmin.xaml
    /// </summary>
    public partial class frmAssignFacAdmin : Window
    {
        User user;
        frmSearchUsers admFrame;

        public frmAssignFacAdmin()
        {
            InitializeComponent();
        }

        public frmAssignFacAdmin(User u, string uid, frmSearchUsers f)
            : this()
        {
            this.user = u;

            this.admFrame = f;
            this.txtUserID.Text = uid;
            WCFHelperClient client = new WCFHelperClient();

            this.txtCurrRole.Text = ((EnumRoles)client.ViewUserRole(uid)).ToString();
            client.Close();

            cboFaculty.ItemsSource = Enum.GetValues(typeof(Faculty));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Check if already an location admin
            if (EnumRoles.Facility_Admin.ToString().CompareTo(txtCurrRole.Text.Trim()) == 0)
            {
                MessageBox.Show("User " + txtUserID.Text + " has already been assigned as " + txtCurrRole.Text,
                    "Already Assigned", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                btnExit_Click(this.btnExit, new RoutedEventArgs());
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            admFrame.Visibility = Visibility.Visible;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();

            try
            {
                if (txtCurrRole.Text.CompareTo(EnumRoles.Nil.ToString()) == 0)
                {
                    client.AssignLocationAdmin(this.user, txtUserID.Text.Trim(), (Faculty)cboFaculty.SelectedIndex);
                }
                else
                {
                    if (MessageBox.Show("Are you sure that you want to overwrite " + txtUserID.Text + "'s Role?",
                        "Confirm Role Change", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        client.AssignLocationAdmin(this.user, txtUserID.Text.Trim(), (Faculty)cboFaculty.SelectedIndex);
                    }
                }


                MessageBox.Show("Role have been added/updated", "Updated Role",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                admFrame.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


    }
}
