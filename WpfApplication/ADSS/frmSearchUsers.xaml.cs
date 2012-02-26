using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;


namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmSearchUsers.xaml
    /// </summary>
    public partial class frmSearchUsers : Window
    {
        User user;
        frmMain mainFrame;
        public frmSearchUsers()
        {
            InitializeComponent();
        }

        public frmSearchUsers(User u, frmMain mainFrame)
            : this()
        {
            this.user = u;
            this.mainFrame = mainFrame;
            this.cboAssign.ItemsSource = System.Enum.GetValues(typeof(EnumRoles));
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                List<User> list;
                if (cboRole.SelectedIndex == 0)
                {
                    list = client.searchUser(txtName.Text.Trim(), txtUserID.Text.Trim()).ToList<User>();
                }
                else
                {
                    list = client.searchUserByRole(txtName.Text.Trim(), txtUserID.Text.Trim(),
                      (EnumRoles)cboRole.SelectedIndex - 1).ToList<User>();
                }
                lstUsers.SelectedValuePath = "userID";
                lstUsers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            if (cboAssign.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a System Role to Assign!");
                return;
            }
            if (lstUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a user!");
                return;
            }
            string uid = lstUsers.SelectedValue.ToString();
            if ((EnumRoles)cboAssign.SelectedIndex == EnumRoles.Nil)
            {
                WCFHelperClient client = new WCFHelperClient();
                try
                {
                    if (MessageBox.Show("Are you sure you want to remove the role of " + uid + "? ",
                        "Confirm Role Removal",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        client.unAssignRole(user, uid);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                client.Close();
            }
            else if ((EnumRoles)cboAssign.SelectedIndex == EnumRoles.Facility_Admin)
            {
                var assignForm = new frmAssignFacAdmin(this.user, uid,  this);
                assignForm.ShowDialog();
            }
            else
            {
                var assignForm = new frmAssign(this.user, uid, (EnumRoles)cboAssign.SelectedIndex, this);
                assignForm.ShowDialog();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboRole.SelectedIndex = 0;
        }


    }
}
